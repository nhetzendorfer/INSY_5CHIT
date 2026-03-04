<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:template match="/">
        <html>
            <head>
                <title>TV Schedule</title>
                <style>
                    body { font-family: Arial; }
                    h1 { color: #333; }
                    h2 { color: #0066cc; }
                    table {margin-left:25%;border-collapse: collapse; width: 50%; margin-bottom:
                    20px; }
                    #table {margin-left:5%;border-collapse: collapse; width: 90%; margin-bottom:
                    20px; }
                    th, td { border: 1px solid #999; padding: 6px; }
                    th { background: #eee; }
                    img {display:flex;justify-self:center}
                </style>
            </head>
            <body>

                <h1> TV Schedule: <xsl:value-of select="TVSCHEDULE/@NAME" />
                </h1>
                <xsl:for-each select="TVSCHEDULE">
                    <xsl:apply-templates match="CHANNEL"/>
                </xsl:for-each>
            </body>
        </html>
    </xsl:template>

    <xsl:template match="PROGRAMSLOT">
        <tr>
            <td>
                <xsl:value-of select="TIME" />
            </td>
            <td>
                <xsl:value-of select="TITLE" />
            </td>
            <td>
                <xsl:value-of select="DESCRIPTION" />
            </td>
        </tr>
    </xsl:template>
    
    <xsl:template match="CHANNEL">
        <table>
            <th>
                <h2>Channel: <xsl:value-of select="@CHAN" /></h2>
            </th>
            <tr>
                <td>
                    <p>
                        <img src="{BANNER}"></img>
                    </p>
                </td>
                <tr>
                </tr>
                <td>
                    <table id="table">
                        <tr>
                            <th>Time</th>
                            <th>Title</th>
                            <th>Description</th>
                        </tr>
                        <xsl:for-each select="DAY">
                            <xsl:apply-templates select="PROGRAMSLOT" />
                        </xsl:for-each>
                    </table>
                    <xsl:value-of select="COPYRIGHT"/>
                </td>
            </tr>
        </table>
    </xsl:template>
   
</xsl:stylesheet>