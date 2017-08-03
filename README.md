# Cable
Type-safe client-server communication for C# featuring [Bridge.NET](https://github.com/bridgedotnet/Bridge) and [NancyFx](https://github.com/NancyFx/Nancy). It is a Remote Procedure Call (RPC) implementation for Bridge.NET as the client and Nancy as the Server.

### [Introducing Cable: Deep Dive Into Building Type-Safe Web Apps in C# with Bridge.NET and Nancy](https://medium.com/@zaid.naom/introducing-cable-deep-dive-into-building-type-safe-web-apps-in-c-with-bridge-net-and-nancy-a65f48398a02)

### [Cable.ArticleSample](https://github.com/Zaid-Ajaj/Cable.ArticleSample) Project used in the article to demonstrate Cable

### [Cable.StandaloneConverter](https://github.com/Zaid-Ajaj/Cable.StandaloneConverter/tree/master) Project that demonstrates using the Cable JSON converter stand-alone without HTTP abstractions

## TL;DR
### On the Nancy Server
Install `Cable.Nancy` from nuget

```
Install-Package Cable.Nancy
```
### Define shared types interfaces in a seperate file `SharedTypes.cs`
```cs
public class Student
{
    public string Name { get; set; }
    public int Id { get; set; }
    public string[] Subjects { get; set; }
    public DateTime DateOfBirth { get; set; }
}

public interface IStudentService
{
    Task<IEnumerable<Student>> GetAllStudents();
    Task<Student> TryFindStudentByName(string name);
}
```
### Register an implementation through a `NancyModule`:
```cs
public class StudentServiceModule : NancyModule
{
    public StudentServiceModule(IStudentService service)
    {
        // automatically generate routes for the provided implementation of IStudentService
        NancyServer.RegisterRoutesFor(this, service);
    }
}
```
### On the Bridge.NET client
Install `Cable.Bridge` from nuget:
```
Install-Package Cable.Bridge
```
Reference the *file* `SharedTypes.cs` into your client project, making the types available.

Create a typed proxy and talk to server directly:
```cs
using System;
using Cable.Bridge;
using ServerSide;

namespace ClientSide
{
    public class Program
    {
        public static async void Main()
        {
            // resolve a typed proxy
            var studentService = BridgeClient.Resolve<IStudentService>();

            // call server just by calling the function
            var students = await studentService.GetAllStudents();

            // use the results directly
            foreach (var student in students)
            {
                var name = student.Name;
                var age = DateTime.Now.Year - student.DateOfBirth.Year;
                Console.WriteLine($"{name} is {age} years old");
            }
        }
    }
}
```



