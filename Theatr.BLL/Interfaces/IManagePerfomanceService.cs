using System;
using System.Collections.Generic;
using Theatr.BLL.DTO;

namespace Theatr.BLL.Interfaces
{
    public interface IManagePerfomanceService
    {
        IEnumerable<PerformanceDTO> FindByTime(DateTime start, DateTime end);
        PerformanceDTO FindPerfomance(int id);
        void AddPerformance(string name, DateTime dateTime, string aut, string genres);
        void DeletePerformance(int id);
        void AddTicket(string ticCat, float ticPrice, int idp);
        IEnumerable<PerformanceDTO> GetAll();
        IEnumerable<AuthorDTO> GetAllAuthors();
        IEnumerable<GenreDTO> GetAllGenres();
        IEnumerable<GenreDTO> GetGenreOfPerformance(string performanceName);
        IEnumerable<GenreDTO> GetGenresByPerformanceId(int id);
        IEnumerable<PerformanceDTO> FindByName(string name);
        IEnumerable<PerformanceDTO> FindByGenreName(string genreName);
        IEnumerable<PerformanceDTO> FindByAuthorName(string authorName);
        void SellTicket(TicketDTO ticketDTO, string userId);
        void SellBronedTicket(TicketDTO ticketDTO);
        TicketDTO GetTicketById(int Id);
    }
}
