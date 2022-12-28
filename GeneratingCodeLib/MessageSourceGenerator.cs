using Microsoft.CodeAnalysis;

namespace Northwind.Shared
{
    [Generator]
    public class MessageSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            IMethodSymbol mainMethod = context.Compilation.GetEntryPoint(context.CancellationToken);
            string sourceCode = $@"// source-generated code
                                                    static partial class {mainMethod.ContainingType.Name}
                                                    {{
                                                      static partial void Message(string message)
                                                      {{
                                                        System.Console.WriteLine($""Generator says: '{{message}}'"");
                                                      }}
                                                    }}";
            string typeName = mainMethod.ContainingType.Name;
            context.AddSource($"{typeName}.Methods.g.cs", sourceCode);

        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // this source generator does not need any initialization
        }
    }
}