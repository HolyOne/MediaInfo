<?xml version="1.0" encoding="ISO-8859-1"?>

<!-- Tahribat.com Media Info XML Reader template -->
<html xsl:version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
<head>
  <title>HolyOne Media Info - Media List</title>
<style type="text/css">
.media
{

border:1px outset gray;
 
 padding:4px ;
}

body{
font-family:Arial;font-size:12pt;background-color:#EEEEEE
}
</style>


</head>
   <!--Media Height="720" Width="1280" VideoCodec="AVC" AudioFormat="AC-3" VideoBitRate="5447000" AudioBitRate="640000" RunTime="77" AudioStreamCount="1" AudioChannelCount="6" AudioProfile="" Subtitles="" VideoFPS="25.000 fps" Filename="After.Sex.2007.BluRay.720p.x264.DD51-MySiLU.mkv" Directory="T:\After Sex" FileType="Video" Extension=".mkv" GroupName="T:\" _Size="3521380670">
      <ExtraFiles>
        <string>After.Sex.2007.BluRay.720p.x264.DD51-MySiLU.srt</string>
      </ExtraFiles>
      <Checked>false</Checked>
    </Media-->


  <body >
    <xsl:for-each select="SaveFile/Medias/Media">
  <div class="media">


   
	<xsl:value-of select="position()"/>
	
  
            
              <!--xsl:value-of select="name()"/-->
          <!--
            <xsl:value-of select="attribute::Filename"/>
                      <xsl:value-of select="attribute::VideoCodec"/>
                      <xsl:value-of select="attribute::AudioFormat"/>
                     <xsl:value-of select="attribute::Directory"/>
		     <xsl:value-of select="attribute::VideoFPS"/>
		     <xsl:value-of select="attribute::_Size"/>
		     <xsl:value-of select="attribute::AudioStreamCount"/>
                      <xsl:value-of select="attribute::AudioChannelCount"/>          
		          <xsl:value-of select="attribute::Subtitles"/>    
			 
-->
     Resolution: <xsl:value-of select="attribute::Height"/>x <xsl:value-of select="attribute::Width"/>
 
<!--
IsSelected<xsl:value-of select="Checked"/>
-->
<xsl:variable name="var" select="Checked"/>
<xsl:if test="$var = 'true'">variable is true</xsl:if>



  -- <xsl:for-each select="ExtraFiles">
   * <xsl:value-of select="string"/> <br/>
  
      </xsl:for-each>
      </div>
</xsl:for-each>
  </body>
</html>
