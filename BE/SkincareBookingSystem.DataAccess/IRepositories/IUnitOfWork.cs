﻿using Microsoft.EntityFrameworkCore.Storage;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface IUnitOfWork
{
    IAppointmentsRepository Appointments { get; }
    IBlogRepository Blog { get; }
    IBlogCategoryRepository BlogCategory { get; }
    IComboItemRepository ComboItem { get; }
    ICustomerRepository Customer { get; }
    ICustomerSkinTestRepository CustomerSkinTest { get; }
    IDurationItemRepository DurationItem { get; }
    IFeedbacksRepository Feedbacks { get; }
    IOrderRepository Order { get; }
    IOrderDetailRepository OrderDetail { get; }
    IServiceComboRepository ServiceCombo { get; }
    IServiceDurationRepository ServiceDuration { get; }
    IServicesRepository Services { get; }
    IServiceTypeRepository ServiceType { get; }
    ISkinProfileRepository SkinProfile { get; }
    ISkinServiceTypeRepository SkinServiceType { get; }
    ISkinTestRepository SkinTest { get; }
    ISkinTherapistRepository SkinTherapist { get; }
    ISlotRepository Slot { get; }
    IStaffRepository Staff { get; }
    ITestAnswerRepository TestAnswer { get; }
    ITestQuestionRepository TestQuestion { get; }
    ITherapistScheduleRepository TherapistSchedule { get; }
    ITypeItemRepository TypeItem { get; }
    IUserManagerRepository UserManager { get; }
    Task<int> SaveAsync();
    
    Task<IDbContextTransaction> BeginTransactionAsync();
}