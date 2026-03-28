using EventBus;
using MassTransit;
using SharedAbstractions.Interfaces;

namespace Account.API.Consumers;

public class UserRegisteredEventConsumer : IConsumer<UserRegisteredEvent>
{
    private readonly ICrudRepository<string, Data.Entities.Account> _repository;

    public UserRegisteredEventConsumer(
        ICrudRepository<string, Data.Entities.Account> repository
    )
    {
        _repository = repository;
    }
    
    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        var account = new Data.Entities.Account
        {
            Id = context.Message.UserId,
            FirstName = context.Message.FirstName,
            LastName = context.Message.LastName,
            Email = context.Message.Email,
            PhoneNumber = context.Message.PhoneNumber,
        };
        
        await _repository.CreateAsync(account);
        
    }
}