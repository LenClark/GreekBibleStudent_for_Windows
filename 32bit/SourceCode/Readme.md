There are actually two "Projects" in the "Solution":
a) the main application project (GreekBibleStudent)
b) an installation project (GBS_32bit_Install)

Note that this was created in Visual Studio 2019.  It may not be accepted by earlier versions and may require some conversion in later versions.

The source files for the main project (GreekBibleStudent) can be found in the 64bit directory.  You will have to rework the Visual Studio solution so that these relate to the 32bit processing rather than 64bit.  (I've not made it more explicit because I think the need for 32 bit code is now a minority need.)

You will need to add the Extension, "Microsoft Visual Studio Installer Projects", for the installation project to work.  If you do not want the installation project (and, hence, do not insdtall the extension), simply create the project from the source files in the 64bit directory but do not add the install project.
