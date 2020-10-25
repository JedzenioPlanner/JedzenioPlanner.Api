using System.Threading;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Application.Common.Interfaces;
using JedzenioPlanner.Api.Application.Recipes.Common;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Recipes;
using MediatR;

namespace JedzenioPlanner.Api.Application.Recipes.Commands.RestoreRecipe
{
    public class RestoreRecipeHandler : IRequestHandler<RestoreRecipe>
    {
        private readonly IEventStore<Recipe> _eventStore;
        private readonly RecipeEventsMapper _mapper;
        private readonly IDateTimeService _dateTime;
        private readonly ICurrentUserService _currentUser;

        public RestoreRecipeHandler(IEventStore<Recipe> eventStore, RecipeEventsMapper mapper, IDateTimeService dateTime, ICurrentUserService currentUser)
        {
            _eventStore = eventStore;
            _mapper = mapper;
            _dateTime = dateTime;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(RestoreRecipe request, CancellationToken cancellationToken)
        {
            await _eventStore.AddEvent(_mapper.Map(request, _dateTime.GetCurrentTime(), _currentUser.GetUserId()));
            return Unit.Value;
        }
    }
}