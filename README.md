# Introduction

This is an old project I found. It's essentially a GUI that can read, edit, and import text files (originally I wanted to use CSV files, but I found it easier to work with TXT files). I made this with the intention of using it to help keep me organized; like allowing me to keep separate to-do piles.

I made this via WPF, and as such, this is Windows only. [Here is the .NET install](https://dotnet.microsoft.com/en-us/download/dotnet-framework). Running the .exe without .NET installed will prompt a download.

# Breakdown of Files

**Images** — Images used in the GUI, and the icon for the exe.

**Project** — All files related to the project itself. You can open this folder directly in VisualStudio yourself if you'd like.

**TextData** — Where all the text files are stored. There's a default one in there, feel free to edit it. The program *is* hardcoded to have that as the default file on launch, but I don't believe this will cause errors as long as you change the file before refreshing.

**Text Holder.exe** — The executable. Double click it to run! The .dll and .json files are required for this to work properly.
