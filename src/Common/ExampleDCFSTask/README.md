# Sample DAS DCFS task implementation

## Add NuGet package

```powershell
Install-Package SFA.DAS.Payments.DCFS
```

## Inherit SFA.DAS.Payments.DCFS.DcfsTask

```csharp
using SFA.DAS.Payments.DCFS;
using SFA.DAS.Payments.DCFS.Context;
```

```csharp
public class CustomTask : DcfsTask
{
    public CustomTask()
        : base("MyTransientSchemaNameForLogging")
    {
    }

    protected override void Execute(ContextWrapper context)
    {
    }
}
```

The base DcfsTask takes a parameter of databaseSchema, which is the SQL schema name that the table TaskLog is in for the component. The DDL can be found in the [/src/SFA.DAS.Payments.DCFS/TaskLog-DDL.sql](../../src/SFA.DAS.Payments.DCFS/TaskLog-DDL.sql)

## Follow the pattern

The above steps are the minimum you need to do to implement a DCFS task. You can add your logic to the execute method directly if you wish; however the Payment team has a pattern of:

* Using StructureMap to provide dependency resolution
* Creating a Processor class to orchestrate business logic
* Use [Mediatr](https://github.com/jbogard/MediatR) to implement commands and queries to provide small, succinct & composable logic.

To follow this pattern, you should:

### Create a Processor

```csharp
public class ExampleProcessor
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public ExampleProcessor(ILogger logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public void Process()
    {
      ...
    }
}
```

### Use a dependency resolver to get an instance of the Processor

```csharp
protected override void Execute(ContextWrapper context)
{
    // Setup the dependency resolver
    _dependencyResolver.Init(typeof(CustomTask), context);

    // Get a processor instance
    var processor = _dependencyResolver.GetInstance<ExampleProcessor>();

    // Do the work
    processor.Process();
}
```

### Create Command / Query

Create a response object (if required). This is used to return data from the query/command
```csharp
public class GetSomeDataQueryResponse : QueryResponse<SomeObject>
{
}
```

Create a request object. This is used to identify handler and pass parameters into the handler.
```csharp
public class GetSomeDataQueryRequest : IRequest<GetSomeDataQueryResponse>
{
}
```

Create a request handler. This is where the logic is implemented.
```csharp
public class GetSomeDataQueryHandler : IRequestHandler<GetSomeDataQueryRequest, GetSomeDataQueryResponse>
{
    public GetSomeDataQueryResponse Handle(GetSomeDataQueryRequest request)
    {
        return new GetSomeDataQueryResponse
        {
            IsValid = true,
            Items = new[]
            {
                new Domain.SomeObject {Id = 1}
            }
        };
    }
}
```

### Orchestrate commands / queries from processor

```csharp
var data = _mediator.Send(new GetSomeDataQueryRequest());
```
