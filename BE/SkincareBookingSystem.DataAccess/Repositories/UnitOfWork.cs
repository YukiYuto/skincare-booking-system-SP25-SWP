using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IUserManagerRepository UserManagerRepository { get; private set; }

    public IAppointmentsRepository Appointments { get; private set; }

    public IBlogRepository Blog { get; private set; }

    public IBlogCategoryRepository BlogCategory { get; private set; }

    public IComboItemRepository ComboItem { get; private set; }

    public ICustomerRepository Customer { get; private set; }

    public ICustomerSkinTestRepository CustomerSkinTest { get; private set; }

    public IDurationItemRepository DurationItem { get; private set; }

    public IFeedbacksRepository Feedbacks { get; private set; }

    public IOrderRepository Order { get; private set; }

    public IOrderDetailRepository OrderDetail { get; private set; }
    
    public IOrderServiceTrackingRepository OrderServiceTracking { get; private set; }

    public IServiceComboRepository ServiceCombo { get; private set; }

    public IServiceDurationRepository ServiceDuration { get; private set; }

    public IServicesRepository Services { get; private set; }

    public IServiceTypeRepository ServiceType { get; private set; }

    public ISkinProfileRepository SkinProfile { get; private set; }

    public ISkinServiceTypeRepository SkinServiceType { get; private set; }

    public ISkinTestRepository SkinTest { get; private set; }

    public ISkinTherapistRepository SkinTherapist { get; private set; }

    public ISlotRepository Slot { get; private set; }

    public IStaffRepository Staff { get; private set; }

    public ITestAnswerRepository TestAnswer { get; private set; }

    public ITestQuestionRepository TestQuestion { get; private set; }

    public ITherapistScheduleRepository TherapistSchedule { get; private set; }
    public ITherapistServiceTypeRepository TherapistServiceType { get; private set; }

    public ITypeItemRepository TypeItem { get; private set; }

    public IUserManagerRepository UserManager { get; private set; }
    public IPaymentRepository Payment { get; }
    public ITransactionRepository Transaction { get; }

    public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        Appointments = new AppointmentsRepository(_context);
        Blog = new BlogRepository(_context);
        BlogCategory = new BlogCategoryRepository(_context);
        ComboItem = new ComboItemRepository(_context);
        Customer = new CustomerRepository(_context);
        CustomerSkinTest = new CustomerSkinTestRepository(_context);
        DurationItem = new DurationItemRepository(_context);
        Feedbacks = new FeedbacksRepository(_context);
        Order = new OrderRepository(_context);
        OrderDetail = new OrderDetailRepository(_context);
        OrderServiceTracking = new OrderServiceTrackingRepository(_context);
        ServiceCombo = new ServiceComboRepository(_context);
        ServiceDuration = new ServiceDurationRepository(_context);
        Services = new ServicesRepository(_context);
        ServiceType = new ServiceTypeRepository(_context);
        SkinProfile = new SkinProfileRepository(_context);
        SkinServiceType = new SkinServiceTypeRepository(_context);
        SkinTest = new SkinTestRepository(_context);
        SkinTherapist = new SkinTherapistRepository(_context);
        Slot = new SlotRepository(_context);
        Staff = new StaffRepository(_context);
        TestAnswer = new TestAnswerRepository(_context);
        TestQuestion = new TestQuestionRepository(_context);
        TherapistSchedule = new TherapistScheduleRepository(_context);
        TherapistServiceType = new TherapistServiceTypeRepository(_context);
        TypeItem = new TypeItemRepository(_context);
        UserManager = new UserManagerRepository(userManager);
        Payment = new PaymentRepository(_context);
        Transaction = new TransactionRepository(_context);
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}