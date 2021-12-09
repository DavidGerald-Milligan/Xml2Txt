# Xml2Txt

###  The Xml2Txt application reads from a wikipedia dump file, and outputs a defined number of files at a defined minimum size.

The ./publish directory contains a small sample data.xml file of 10MB in size to get you moving.

### publish the project, it should pick up the publish profile at Xml2Txt\Properties\PublishProfiles\FolderProfile3.pubxml

###  Download a dump file 
.xml.bz2 compressed files can be downloaded from https://dumps.wikimedia.org/enwiki/

Example:/20210901/enwiki-20210901-pages-articles-multistream.xml.bz2

This is an 18GB compressed file containing  some 25,000,0000 wikipedia pages.

Use a utility such as [7-zip](https://www.7-zip.org/download.html) to extract this to enwiki-20210901-pages-articles-multistream.xml,

Rename the extracted file to something simpler, like data.xml.

The extracted file is now some 90GB.

Copy the ../publish/Xml2Txt.exe to the same directory as the .xml file.

Click on Xml2Txt.exe and you will be prompted for:
- The number of files to create.
- The minimum size of each file in MB.
- The data file to use (maybe data.xml).
- A directory to add files to.

The application will read each page from the xml, when it has read content greater than the minimum file size it will create a file in the defined directory.

Example:
5 files, min size 10 MB, using data.xml as input, writing to directory "BIG" will produce:
- BIG
	- BIG_1.txt
	- BIG_2.txt
	- BIG_3.txt
	- BIG_4.txt
	- BIG_5.txt

