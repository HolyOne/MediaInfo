<?xml version="1.0" encoding="utf-8"?>
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xsl:version="1.0">

<head>
<meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
<title>HolyOne Media Info - Media List</title>
<style type="text/css">
body {
	font-family: tahoma;
	font-size: 0.8em;
	background-color: #EEEEEE;
}
h1, h2, h3 {
color:#036;
	line-height: 0.8em;
}
div.generated {
	color: green;
	text-align: right; 
}
.Media {
	background-color: #fff;
	padding: 4px;
}
.Media div.hdr {
	font-weight: bold;
	background-color: #036;
	color: white;
	width: 100%;
	padding: 4px 4px 4px 4px;
}
.Media.content {
	background-color: #eee;
	width: 100%;
}
table {
	border: 1px outset gray;
	width: 100%;
	font-size: 1em;
}
tr {
	text-align: left;
	margin: 0 0 0 0;
}
td {
	padding: 4px 4px 4px 4px;
	border: 0px;
	text-align: left;
}
th {
	text-align: left;
}
tr:nth-child(even) {background: #ddd}
tr:nth-child(odd) {background: #eee}
li{ line-height:0.8em;}

</style>
</head>

<body>

<h2>Tahribat.com MediaInfo V1.2 Media List</h2>
<div class="generated">
	Generated on: <xsl:value-of select="SaveFile/attribute::GenerateTime" />
</div>

<xsl:for-each select="SaveFile/Medias/Media">





<div class="Media">
	<div class="hdr">
	<xsl:variable name="var" select="Checked" />
	<xsl:choose>
	<xsl:when test="$var = 'true'">
	<div style="float:right; font-weight:normal; font-size:1.1em"> ☑</div> 
	</xsl:when>
	<xsl:otherwise>
	</xsl:otherwise>
	</xsl:choose>


	<xsl:value-of select="position()" />)  <xsl:value-of select="attribute::Filename" />
	

	</div>
	<table class="dtl">
		<tr>
			<th>Resolution</th>
			<td>  <xsl:value-of select="attribute::Height" />x <xsl:value-of select="attribute::Width" />
			</td>
			<th>VideoCodec</th>
			<td><xsl:value-of select="attribute::VideoCodec" /> </td>
			<th>VideoFPS:</th>
			<td>  <xsl:value-of select="attribute::VideoFPS" /> </td>
			<th>AudioFormat</th>
			<td>  <xsl:value-of select="attribute::AudioFormat" /> </td>
		</tr>
		<tr>
			<th>AudioChannelCount</th>
			<td>  <xsl:value-of select="attribute::AudioChannelCount" /> </td>
			<th>AudioStreamCount</th>
			<td>  <xsl:value-of select="attribute::AudioStreamCount" /> </td>
			<th>Size</th>
			<td>
			
<xsl:variable name="size" select="attribute::_Size" />
 
        <xsl:choose>     <xsl:when test="$size >= 1099511627776">
                        <xsl:value-of select="format-number($size div 1099511627776,'#,###')"/>
                        <xsl:text> TB</xsl:text>
</xsl:when>
      <xsl:when test="$size >= 1073741824">
                        <xsl:value-of select="format-number($size div 1073741824,'#,###')"/>
                        <xsl:text> GB</xsl:text>
                </xsl:when>
                <xsl:when test="$size >= 1048576">
                        <xsl:value-of select="format-number($size div 1048576,'#,###')"/>
                        <xsl:text> MB</xsl:text>
                </xsl:when>
                <xsl:when test="$size >= 1024">
                        <xsl:value-of select="format-number($size div 1024,'#,###')"/>
                        <xsl:text> KB</xsl:text>
                </xsl:when>

                <xsl:otherwise>
		<xsl:value-of select="format-number($size,'#,###')"/>
                       <xsl:text> Bytes</xsl:text>
		       
                </xsl:otherwise>
        </xsl:choose>
 
			 </td>
			<th>RunTime</th>
			<td><xsl:value-of select="attribute::RunTime" /> minutes </td>
		</tr>


<xsl:if test="attribute::Subtitles!=''">
		<tr>
			<th>Subtitles</th>
			<td colspan="7"> 
 <xsl:value-of select="attribute::Subtitles" />
			</td>
		</tr>
</xsl:if>

<xsl:if test="ExtraFiles/string/text()">
		<tr>
			<th>ExtraFiles</th>
			<td colspan="7">
			<ul compact="compact">
  <xsl:for-each select="ExtraFiles">
        	    <li> <xsl:value-of select="string" /></li>
  </xsl:for-each>
      	    </ul>
			</td>
		</tr>
</xsl:if>


    </table>
</div>


</xsl:for-each>
<hr />Generated by HolyOne Media Info <i>(http://www.tahribat.com)</i>. You can 
open this file in HolyOne Media Info,
<a href="http://www.tahribat.com/MediaInfo.asp">Download from tahribat.com</a>.

</body>

</html>