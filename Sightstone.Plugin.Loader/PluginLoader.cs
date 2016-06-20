using Sightstone.Plugin.HostViewAddIn;
using System;
using System.AddIn.Hosting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace Sightstone.Plugin.Loader
{
    public class PluginLoader
    {
        public PluginLoader()
        {
            var directoryInfo = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);
            var addInRootPath = Path.Combine(directoryInfo.Parent.FullName, "Plugin");

            string[] warnings = AddInStore.Update(addInRootPath);
            DisplayWarnings(warnings);

            Collection<AddInToken> tokens = AddInStore.FindAddIns(typeof(ISightstonePlugin), addInRootPath);
            DisplayTokens(tokens);

            foreach (AddInToken token in tokens)
            {
                var grantSet = new PermissionSet(PermissionState.None);
                grantSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
                grantSet.AddPermission(new FileIOPermission(FileIOPermissionAccess.AllAccess, Path.Combine(Assembly.GetExecutingAssembly().Location, "Plugin", token.AddInFullName)));
                grantSet.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess));
                ISightstonePlugin calculatorPlugin = token.Activate<ISightstonePlugin>(AddInSecurityLevel.FullTrust);
            }
            
        }

        private static void DisplayWarnings(IEnumerable<string> warnings)
        {
            foreach (var warning in warnings)
            {
                Console.WriteLine(warning);
            }
        }

        private static void DisplayTokens(Collection<AddInToken> tokens)
        {
            foreach(var token in tokens)
            {
                Console.WriteLine(
                    $"{token.Name} - {token.AddInFullName}\t {token.AssemblyName}\t {token.Description}\t {token.Version}");
            }
        }
    }
}
