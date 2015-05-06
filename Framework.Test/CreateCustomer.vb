Imports LazyFramework.CQRS.EventHandling
Imports LazyFramework.CQRS.Command
Imports LazyFramework.CQRS.Logging
Imports NUnit.Framework

Public Class CustomerCommandBase
    Inherits CommandBase(Of Customer)
    Public Id As Guid
End Class

Public Class CreateCustomerCommand
    Inherits CustomerCommandBase

    Public Name As String
    Public Address As String
End Class

Public Class UpdateCustomerNameCommand
    Inherits CustomerCommandBase

    Public NewName As String

End Class


Public Class Customer

    Property Id As Guid
    Property Name As String
    Property Address As String

    Public Phone As String
    Public Country As String
    Public CustomerLevel As Integer

End Class

Public Class CustomerCreatedEvent
    Inherits CQRS.EventHandling.EventBase

    Public Id As Guid
    Public Name As String
    Public Address As String
    Public CustomerLevel As Integer

    Public Sub New(customer As Customer)
        Id = customer.Id
        Name = customer.Name
        Address = customer.Address
        CustomerLevel = customer.CustomerLevel
    End Sub


End Class


Public Class CustomerAlteredEvent
    Inherits CQRS.EventHandling.EventBase

    Public ReadOnly ID As Guid

    Public Sub New(id As Guid)
        Me.ID = id
    End Sub

End Class

Public Class HandleCustomerEvents
    Implements CQRS.EventHandling.IHandleEvent

    Public Shared Sub CustomerCreated(e As CustomerCreatedEvent)

    End Sub
End Class

Public Class CommandHandler
    Implements CQRS.Command.IHandleCOmmand, CQRS.EventHandling.IPublishEvent

    <ThreadStatic> Public Shared CustomerRepository As New Dictionary(Of Guid, Customer)

    <CQRS.EventHandling.PublishesEventOfType(GetType(CustomerCreatedEvent))>
    Public Shared Sub CreateCustomer(cust As CreateCustomerCommand)
        'Save customer to databse

        Dim customer As New Customer
        customer.Id = cust.Id
        customer.Name = cust.Name
        customer.Address = cust.Address
        customer.CustomerLevel = 0

        CommandHandler.CustomerRepository.Add(customer.Id, customer)

        CQRS.EventHandling.EventHub.Publish(cust, New CustomerCreatedEvent(customer))

    End Sub

    Public Shared Sub AlterName(cmd As UpdateCustomerNameCommand)
        CustomerRepository(cmd.Id).Name = cmd.NewName
        CQRS.EventHandling.EventHub.Publish(cmd, New CustomerAlteredEvent(cmd.Id))
    End Sub

End Class


Public Class EventHandler
    Implements IHandleEvent

    Public Shared Sub SaveCustomer(custom As CustomerCreatedEvent)

    End Sub


End Class


Public Class Persistdata
    Implements ILogWriter

    Public Sub WriteCommand(cmd As LazyFramework.CQRS.Logging.CommandInfo, orginalcommand As IAmACommand) Implements ILogWriter.WriteCommand
        Debug.WriteLine(cmd.GetType.FullName)
    End Sub

    Public Sub WriteError(erroInfo As ErrorInfo) Implements ILogWriter.WriteError

    End Sub

    Public Sub WriteEvent(cmd As EventInfo) Implements ILogWriter.WriteEvent
        Debug.WriteLine(cmd.CommandData)
        Debug.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId)
        EventList.Add(cmd.CommandData)
    End Sub

    Public Sub WriteQuery(cmd As QueryInfo) Implements ILogWriter.WriteQuery

    End Sub

    Public Property EventList As New List(Of CQRS.Command.IAmACommand)

End Class
