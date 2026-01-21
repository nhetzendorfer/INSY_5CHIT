using SimplecMethod;
using SimplecMethod.DbContext;
using SimplexMethod;

namespace beer;

class Program
{
    static void Main(string[] args)
    {
        var ctx = new SimplexContext();

        var service = new OptimizationService(ctx);
        var result = service.CalculateOptimalProduction();
        
        result.matrixs[0].ToString();
        foreach (var simplexSnap in result.matrixs)
        {
            foreach (var row in simplexSnap.matrix)
            {
                foreach (var col in row)
                {
                    Console.Write(Math.Round(col, 4)+"\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
        }
        
        Console.WriteLine($"Status: {result.Status}");
        Console.WriteLine("Optimale Produktion (mL):");
        foreach (var kv in result.ProductionPerUnit)
        {
            Console.WriteLine($" - {kv.Key}: {kv.Value} mL");
        }
        Console.WriteLine($"Gesamtprofit: {result.TotalProfit}");
        }
    }

    public struct SResult
    {
        public SResult()
        {
            TotalProfit = 0;
        }
        public Dictionary<string, double> ProductionPerUnit { get; set; } = new Dictionary<string, double>();
        
        public double TotalProfit { get; set; }
        
        public string Status { get; set; } = "Unknown";

        public List<SimplexSnap> matrixs=new ();
    }

    public class OptimizationService
    {
        private readonly SimplexContext _context;

        public OptimizationService(SimplexContext context)
        {
            _context = context;
        }
        
        public SResult CalculateOptimalProduction()
        {
            var beers = _context.ProductTypes
                .OrderBy(b => b.Id)
                .ToList();

            var resources = _context.Resources
                .OrderBy(r => r.Id)
                .ToList();

            var requirements = _context.ProductRequirments
                .ToList(); 

            var stocks = _context.ResourceStocks
                .ToList(); 

            
            if (beers.Count == 0 || resources.Count == 0)
            {
                return new SResult
                {
                    Status = "NoData"
                };
            }
            
            var beerIdToIndex = new Dictionary<int, int>();
            for (int i = 0; i < beers.Count; i++) beerIdToIndex[beers[i].Id] = i;

            var resourceIdToIndex = new Dictionary<int, int>();
            for (int i = 0; i < resources.Count; i++) resourceIdToIndex[resources[i].Id] = i;

            int nVars = beers.Count;          
            int nConstraints = resources.Count; 

            double[] functionVars = new double[nVars];
            for (int i = 0; i < nVars; i++)
            {
                
                functionVars[i] = Convert.ToDouble(beers[i].ProfitPreHl);
            }

            
            Function function = new Function(functionVars, 0.0, true);

            Constraint[] constraints = new Constraint[nConstraints];

            for (int i = 0; i < nConstraints; i++)
            {
                var resource = resources[i];

                double[] coeffs = new double[nVars];
                for (int j = 0; j < nVars; j++) coeffs[j] = 0.0;

                
                var reqsForResource = requirements.Where(rq => rq.ResourceId == resource.Id);
                foreach (var req in reqsForResource)
                {
                    if (beerIdToIndex.TryGetValue(req.ProductId, out int beerIdx))
                    {
                        coeffs[beerIdx] = Convert.ToDouble(req.UnitsRequired);
                    }
                }

               
                var stock = stocks.FirstOrDefault(s => s.ResourceId == resource.Id);
                double b = stock != null ? Convert.ToDouble(stock.AvailibleUnits) : 0.0;

                
                constraints[i] = new Constraint(coeffs, b, "<=");
            }

            // Simplex
            var simplex = new Simplex(function, constraints);
            var resultTuple = simplex.GetResult();
            var snaps = resultTuple.Item1;
            var result = resultTuple.Item2;

            var dto = new SResult
            {
                Status = result.ToString()
            };

            if (result == SimplexResult.Found)
            {
                var finalSnap = snaps.Last();
                
                int totalVariables = finalSnap.matrix.Length;
                
                double[] variableValues = new double[totalVariables];
                for (int i = 0; i < totalVariables; i++)
                {
                    int rowIndex = Array.IndexOf(finalSnap.C, i);
                    if (rowIndex != -1 && rowIndex < finalSnap.b.Length)
                        variableValues[i] = finalSnap.b[rowIndex];
                    else
                        variableValues[i] = 0.0;
                }
                
                for (int i = 0; i < nVars; i++)
                {
                    string name = beers[i].Name;
                    double valueHl = variableValues[i];
                    dto.ProductionPerUnit[name] = Math.Round(valueHl, 6);
                }

                
                double totalProfit = 0.0;
                for (int i = 0; i < nVars; i++)
                {
                    totalProfit += functionVars[i] * (dto.ProductionPerUnit.ContainsKey(beers[i].Name) ? dto.ProductionPerUnit[beers[i].Name] : 0.0);
                }
                dto.TotalProfit = Math.Round(totalProfit, 6);
                dto.matrixs = snaps;
            }
            else if (result == SimplexResult.Unbounded)
            {
                dto.TotalProfit = double.NaN;
            }
            else
            {
                dto.TotalProfit = double.NaN;
            }
            
            return dto;
        }
    }