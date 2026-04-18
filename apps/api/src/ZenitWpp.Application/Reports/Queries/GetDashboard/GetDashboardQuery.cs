using MediatR;
using ZenitWpp.Application.Reports.DTOs;

namespace ZenitWpp.Application.Reports.Queries.GetDashboard;

public record GetDashboardQuery : IRequest<DashboardResponse>;
