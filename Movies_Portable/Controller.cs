using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sumo.Serialization.Web;

namespace Movies_Portable
{
    public class Controller
    {
        private string mURLBase = "https://api.themoviedb.org/3/";
        private string mAPIKey = "a664be023cff8abc2bc7246cef87997d";
        private int mCurrentPage = 0;

        public string userInputTitle { get; set; }
        public string userInputYear { get; set; }
        
        

        public async Task<List<MovieDetails>> getNextPage()
        {
            List<MovieDetails> lResult = null;
            string lBuiltUrl = string.Empty;
            mCurrentPage++;

            if (userInputTitle != string.Empty && userInputYear == string.Empty)
            {
                lBuiltUrl = mURLBase + "search/movie?api_key=" + mAPIKey + "&query=" + userInputTitle + "&page=" + mCurrentPage.ToString();
            }
            else if (userInputTitle == string.Empty && userInputYear != string.Empty)
            {
                lBuiltUrl = mURLBase + "discover/movie?api_key=" + mAPIKey + "&primary_release_year=" + userInputYear + "&page=" + mCurrentPage.ToString();
            }
            else if (userInputTitle != string.Empty && userInputYear != string.Empty)
            {
                lBuiltUrl = mURLBase + "search/movie?api_key=" + mAPIKey + "&query=" + userInputTitle + "&primary_release_year=" + userInputYear + "&page=" + mCurrentPage.ToString();
            }
            else
            {
                lBuiltUrl = mURLBase + "movie/now_playing?api_key=" + mAPIKey + "&page=" + mCurrentPage.ToString();

            }


            Serializer lSerializer = new Serializer();
            SearchResult lSearch = await lSerializer.Read<SearchResult>(lBuiltUrl, SourceType.Json);

            if (lSearch != null && lSearch.results != null)
            {
                lResult = lSearch.results;
            }

            return lResult;
        }

        public void resetPageCount()
        {
            mCurrentPage = 0;
        }

    }
}
