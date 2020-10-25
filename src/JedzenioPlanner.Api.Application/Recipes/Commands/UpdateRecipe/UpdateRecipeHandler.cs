using System.Threading;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Application.Common;
using JedzenioPlanner.Api.Application.Common.Interfaces;
using JedzenioPlanner.Api.Application.Recipes.Common;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Recipes;
using MediatR;

namespace JedzenioPlanner.Api.Application.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeHandler : IRequestHandler<UpdateRecipe>
    {
        private readonly IEventStore<Recipe> _eventStore;
        private readonly RecipeEventsMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        private readonly IDateTimeService _dateTime;

        public UpdateRecipeHandler(IEventStore<Recipe> eventStore, RecipeEventsMapper mapper, ICurrentUserService currentUser, IDateTimeService dateTime)
        {
            _eventStore = eventStore;
            _mapper = mapper;
            _currentUser = currentUser;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(UpdateRecipe request, CancellationToken cancellationToken)
        {
            var @event = _mapper.Map(request, _dateTime.GetCurrentTime(), _currentUser.GetUserId());
            await _eventStore.AddEvent(@event);
            return Unit.Value;
        }
    }
}