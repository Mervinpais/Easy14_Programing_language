using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace LIM_package_manager
{
    internal class CompileMainProject
    {
        public static void CompileCSharpProject(string projectDirectory, string outputDirectory)
        {
            try
            {
                // Set up compilation options
                var compilationOptions = new CSharpCompilationOptions(OutputKind.ConsoleApplication);

                // Load C# project by reading the source files
                var sourceFiles = Directory.GetFiles(projectDirectory, "*.cs", SearchOption.AllDirectories);
                var syntaxTrees = sourceFiles.Select(file => CSharpSyntaxTree.ParseText(File.ReadAllText(file)));

                // Get references to other assemblies (if required)
                // For example, to reference System.dll:
                // var references = new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) };

                // Create C# compilation
                var compilation = CSharpCompilation.Create(
                    Path.GetFileName(outputDirectory),
                    syntaxTrees,
                    references: null, // Pass the references here if required
                    options: compilationOptions
                );

                // Perform compilation
                var result = compilation.Emit(Path.Combine(outputDirectory, "Easy14 Language Setup\\Easy14\\Easy14_Programming_Language.exe"));

                // Check for compilation errors
                if (!result.Success)
                {
                    Console.WriteLine("Compilation failed:");
                    foreach (var diagnostic in result.Diagnostics)
                    {
                        Console.WriteLine(diagnostic);
                    }
                }
                else
                {
                    Console.WriteLine("Compilation successful!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during compilation: {ex.Message}");
            }
        }
    }
}