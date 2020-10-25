using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using JedzenioPlanner.Api.Application.Common.Behaviors;
using JedzenioPlanner.Api.ApplicationTests.Common.Behaviors.Common;
using Xunit;
using ValidationException = JedzenioPlanner.Api.Application.Common.Exceptions.eru.Application.Common.Exceptions.ValidationException;

namespace JedzenioPlanner.Api.ApplicationTests.Common.Behaviors
{
    internal class SampleValidator : AbstractValidator<SampleRequest>
    {
        public SampleValidator()
        {
            RuleFor(x => x.Version)
                .NotEmpty().WithMessage("Version cannot be empty.");
        }
    }
    
    public class ValidationBehaviorTests
    {
        [Fact]
        public Task DoesValidationBehaviorWorksCorrectlyWhenNoValidators()
        {
            var validators = new IValidator<SampleRequest>[]{};
            var behavior = new ValidationBehavior<SampleRequest, SampleResponse>(validators);
            var request = new SampleRequest
            {
                Version = "v2.0",
                IsWorking = true
            };

            Action a = () => behavior.Handle(request, CancellationToken.None,
                () => Task.FromResult(new SampleResponse {HasWorked = true})).GetAwaiter().GetResult();

            a.Should().NotThrow();
            return Task.CompletedTask;
        }

        [Fact]
        public Task DoesValidationBehaviorWorksCorrectlyWhenAllValidatorsPass()
        {
            var validators = new IValidator<SampleRequest>[]{new SampleValidator()};
            var behavior = new ValidationBehavior<SampleRequest, SampleResponse>(validators);
            var request = new SampleRequest
            {
                Version = "v2.0",
                IsWorking = true
            };

            Action a = () => behavior.Handle(request, CancellationToken.None,
                () => Task.FromResult(new SampleResponse {HasWorked = true})).GetAwaiter().GetResult();

            a.Should().NotThrow();
            return Task.CompletedTask;
        }

        [Fact]
        public Task DoesValidationBehaviorWorksCorrectlyWhenValidatorDoesNotPass()
        {
            var validators = new IValidator<SampleRequest>[]{new SampleValidator(), };
            var behavior = new ValidationBehavior<SampleRequest, SampleResponse>(validators);
            var request = new SampleRequest
            {
                IsWorking = true
            };

            Action a = () => behavior.Handle(request, CancellationToken.None,null).GetAwaiter().GetResult();

            a.Should()
                .Throw<ValidationException>();

            return Task.CompletedTask;
        }
    }
}