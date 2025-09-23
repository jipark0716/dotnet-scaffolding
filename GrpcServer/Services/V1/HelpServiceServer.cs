using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Lib.MTlsManager;
using V1.Help;

namespace GrpcServer.Services.V1;

public class HelpServiceServer(
    HelpService.AppContext appContext
) : global::V1.Help.HelpService.HelpServiceBase {
    
    public override Task<HelpResponse> Health(Empty request, ServerCallContext context)
    {
        IMTlsSessionContext? mTlsSessionContext = context.GetHttpContext().Features.Get<IMTlsSessionContext>();
        Console.WriteLine(mTlsSessionContext?.ConnectionId);
        
        return Task.FromResult(new HelpResponse
        {
            Health = true,
            Uptime = Duration.FromTimeSpan(appContext.Uptime()),
        });
    }
}