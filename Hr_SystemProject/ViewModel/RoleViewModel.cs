using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HR_SystemProject.ViewModel
{
    public class RoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
        public List<CheckBox> checkBox { get; set; }
        public List<string>? ControllerNames { get; set; }
        
    }
    public class ControllerData
    {
        public string ControllerName { get; set; }
        public List<ActionData> Actions{ get; set; }
        
    }
    public class ActionData
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string DisplayValue
            => $"Permissions.{ControllerName}.{ActionName}";
    }
    public class Permissions
    {
        public List<ControllerData> controllerDatas { get; set; } = Permissions.GenerateControllerActionList();
        

        public static List<ControllerData> GenerateControllerActionList()
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            var controllerlist = asm.GetTypes()
           .Where(type => typeof(Controller).IsAssignableFrom(type)) //filter controllers
           .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
           .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)) && !method.IsDefined(typeof(HttpPostAttribute)))
           .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name })
           .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList()
           .GroupBy(x => x.Controller).ToList();
            
            List<ControllerData> controllers = new List<ControllerData>();
            foreach (var controller in controllerlist)
            {
                List<ActionData> actions = new List<ActionData>();
                foreach (var action in controller)
                {
                    actions.Add(new ActionData() { ControllerName = action.Controller, ActionName = action.Action });
                }

                 controllers.Add(new ControllerData() { ControllerName = controller.Key, Actions = actions });

            }
            return controllers;
        }
    }
    public class CheckBox
    {
        public string DisplayValue { get; set; }
        public bool IsChecked { get; set; }
        
    }
}
