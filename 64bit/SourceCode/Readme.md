These files (other than this readme) must be placed in a directory called GreekBibleStudent (no spaces).

There are actually two "Projects" in the "Solution":
a) the main application project (GreekBibleStudent)
b) an installation project (GBS_Install)

Note that this was created in Visual Studio 2019.  It may not be accepted by earlier versions and may require some conversion in later versions.

You will need to add the Extension, "Microsoft Visual Studio Installer Projects", for the installation project to work.  If you do not want the installation project (and, hence, do not insdtall the extension):
a) go ahead as is (including the GBS_Install project);
b) Open the sln file, which will give you an error message
c) remove the GBS_Install project from within Visual Studio.
This should remove all errors and allow you to continue with the main application.
