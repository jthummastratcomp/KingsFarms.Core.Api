using HotTowel.Core.Api.Enums;
using HotTowel.Core.Api.ViewModels;

namespace HotTowel.Core.Api.Services.Interfaces;

public interface IHorseManureService
{
    List<ManureLoadViewModel> GetManureLoadForMonth(MonthEnum loadMonth);
}