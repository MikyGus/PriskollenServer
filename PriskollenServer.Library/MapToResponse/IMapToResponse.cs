namespace PriskollenServer.Library.MapToResponse;
public interface IMapToResponse<TModel, TResponse>
{
    IEnumerable<TResponse> MapToResponse(IEnumerable<TModel> models);
    TResponse MapToResponse(TModel model);
}