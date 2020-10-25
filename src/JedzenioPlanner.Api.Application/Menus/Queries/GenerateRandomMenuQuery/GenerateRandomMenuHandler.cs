using System.Threading;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Application.Common.Interfaces;
using JedzenioPlanner.Api.Domain.MealPlan;
using MediatR;

namespace JedzenioPlanner.Api.Application.Menus.Queries.GenerateRandomMenuQuery
{
    public class GenerateRandomMenuHandler : IRequestHandler<GenerateRandomMenu, MealPlan>
    {
        private readonly IMenuGenerator _generator;

        public GenerateRandomMenuHandler(IMenuGenerator generator)
        {
            _generator = generator;
        }

        public async Task<MealPlan> Handle(GenerateRandomMenu request, CancellationToken cancellationToken)
        {
            var menu = await _generator.GenerateMenu(Mapper.Map(request));
            menu.StripMetadata();
            return menu;
        }
    }
}