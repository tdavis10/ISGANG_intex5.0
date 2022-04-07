// Author IS GANG
// This is our home controller

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AuthLab2_RyanPinkney.Models;
using AuthLab2_RyanPinkney.Models.ViewModels;
using System.IO;
using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using Amazon;
using Amazon.Runtime;

namespace AuthLab2_RyanPinkney.Controllers
{
    public class HomeController : Controller
    {
        //public string GetSecret()
        //{
        //    string secretName = "arn:aws:secretsmanager:us-east-1:100931026615:secret:connectionstringsecret-by6QkU";
        //    string region = "us-east-1";
        //    string secret = "";

        //    MemoryStream memoryStream = new MemoryStream();

        //    IAmazonSecretsManager client = new AmazonSecretsManagerClient(new StoredProfileAWSCredentials());

        //    GetSecretValueRequest request = new GetSecretValueRequest();
        //    request.SecretId = secretName;
        //    request.VersionStage = "AWSCURRENT"; // VersionStage defaults to AWSCURRENT if unspecified.

        //    GetSecretValueResponse response = null;

        //    // In this sample we only handle the specific exceptions for the 'GetSecretValue' API.
        //    // See https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
        //    // We rethrow the exception by default.

        //    try
        //    {
        //        response = client.GetSecretValueAsync(request).Result;
        //    }
        //    catch (DecryptionFailureException e)
        //    {
        //        // Secrets Manager can't decrypt the protected secret text using the provided KMS key.
        //        // Deal with the exception here, and/or rethrow at your discretion.
        //        throw;
        //    }
        //    catch (InternalServiceErrorException e)
        //    {
        //        // An error occurred on the server side.
        //        // Deal with the exception here, and/or rethrow at your discretion.
        //        throw;
        //    }
        //    catch (InvalidParameterException e)
        //    {
        //        // You provided an invalid value for a parameter.
        //        // Deal with the exception here, and/or rethrow at your discretion
        //        throw;
        //    }
        //    catch (InvalidRequestException e)
        //    {
        //        // You provided a parameter value that is not valid for the current state of the resource.
        //        // Deal with the exception here, and/or rethrow at your discretion.
        //        throw;
        //    }
        //    catch (ResourceNotFoundException e)
        //    {
        //        // We can't find the resource that you asked for.
        //        // Deal with the exception here, and/or rethrow at your discretion.
        //        throw;
        //    }
        //    catch (System.AggregateException ae)
        //    {
        //        // More than one of the above exceptions were triggered.
        //        // Deal with the exception here, and/or rethrow at your discretion.
        //        throw;
        //    }

        //    // Decrypts secret using the associated KMS key.
        //    // Depending on whether the secret is a string or binary, one of these fields will be populated.
        //    if (response.SecretString != null)
        //    {
        //        secret = response.SecretString;
        //    }
        //    else
        //    {
        //        memoryStream = response.SecretBinary;
        //        StreamReader reader = new StreamReader(memoryStream);
        //        string decodedBinarySecret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
        //    };

        //    return secret;
        //}

        // Bring in the database
        private ICrashRepository repo { get; set; }


        public HomeController(ICrashRepository temp)
        {
            repo = temp;
        }

        // First route for the index
        public IActionResult Index()
        {
            return View();
        }

        // Route for the privacy page
        public IActionResult Privacy()
        {

            //ViewBag.Secret = GetSecret();

            return View();
        }

        // Get route for the summary page
        [HttpGet]
        public IActionResult Summary(string cityNames, int iPageNum = 1)
        {
            // Set the number of results
            int iPageSize = 50;

            // Create an instance of the accident view models
            var x = new AccidentViewModels
            {
                // Create the accident list
                Accidents = repo.Accidents
                .Where(p => p.county_name == cityNames || cityNames == null)
                //.OrderBy(p => p.CITY)
                .Skip(iPageSize * (iPageNum - 1))
                .Take(iPageSize),

                // Set page info
                PageInfo = new PageInfo
                {   
                    iTotalProjectsNum = (cityNames == null
                    ? repo.Accidents.Count() : repo.Accidents.Where(x => x.county_name == cityNames).Count()),
                    iProjectsPerPage = iPageSize,
                    iCurrentPage = iPageNum
                }

            };

            return View(x);
        }

        // Get route for the summary firt
        [HttpGet]
        public IActionResult SummaryFirst(string cityNames, int iPageNum = 1)
        {
            // Set the number of results
            int iPageSize = 50;

            // Set the accident view model
            var x = new AccidentViewModels
            {
                // Get data to display
                Accidents = repo.Accidents
                .Where(p => p.county_name == cityNames || cityNames == null)
                //.OrderBy(p => p.CITY)
                .Skip(iPageSize * (iPageNum - 1))
                .Take(iPageSize),

                PageInfo = new PageInfo
                {
                    iTotalProjectsNum = (cityNames == null
                    ? repo.Accidents.Count() : repo.Accidents.Where(x => x.county_name == cityNames).Count()),
                    iProjectsPerPage = iPageSize,
                    iCurrentPage = 1
                }
            };

            return View("Summary", x);
        }

        // Get route for the summary previous page
        [HttpGet]
        public IActionResult SummaryPrevious(string cityNames, int id)
        {
            // Set the number of results
            int iPageSize = 50;


            // Set the accident view model
            var x = new AccidentViewModels
            {
                Accidents = repo.Accidents
                .Where(p => p.county_name == cityNames || cityNames == null)
                //.OrderBy(p => p.CITY)
                .Skip(iPageSize * (id - 1))
                .Take(iPageSize),

                PageInfo = new PageInfo
                {
                    iTotalProjectsNum = (cityNames == null
                    ? repo.Accidents.Count() : repo.Accidents.Where(x => x.county_name == cityNames).Count()),
                    iProjectsPerPage = iPageSize,
                    iCurrentPage = id - 10
                }

            };

            return View("Summary", x);
        }

        // Get route for the summary next button
        [HttpGet]
        public IActionResult SummaryNext(string cityNames, int id)
        {
            // Set the number of results
            int iPageSize = 50;


            // Set the accident view model
            var x = new AccidentViewModels
            {
                Accidents = repo.Accidents
                .Where(p => p.county_name == cityNames || cityNames == null)
                //.OrderBy(p => p.CITY)
                .Skip(iPageSize * (id - 1))
                .Take(iPageSize),

                PageInfo = new PageInfo
                {
                    iTotalProjectsNum = (cityNames == null
                    ? repo.Accidents.Count() : repo.Accidents.Where(x => x.county_name == cityNames).Count()),
                    iProjectsPerPage = iPageSize,
                    iCurrentPage = id + 10
                }

            };

            return View("Summary", x);
        }

        // Get route for the summary last button
        [HttpGet]
        public IActionResult SummaryLast(string cityNames, int iPageNum = 1)
        {
            // Set the number of results
            int iPageSize = 50;

            // Set the accident view models
            var x = new AccidentViewModels
            {
                Accidents = repo.Accidents
                .Where(p => p.county_name == cityNames || cityNames == null)
                //.OrderBy(p => p.CITY)
                .Skip(iPageSize * (iPageNum - 1))
                .Take(iPageSize),

                // Set the page info
                PageInfo = new PageInfo
                {
                    iTotalProjectsNum = (cityNames == null
                    ? repo.Accidents.Count() : repo.Accidents.Where(x => x.county_name == cityNames).Count()),
                    iProjectsPerPage = iPageSize,
                    iCurrentPage = (int)Math.Ceiling(((double)(cityNames == null
                    ? repo.Accidents.Count() : repo.Accidents.Where(x => x.county_name == cityNames).Count()) / iPageSize)) - 9
                }

            };

            return View("Summary", x);
        }


        // Route for the severity model
        public IActionResult SeverityModel()
        {
            return View();
        }

        // Route for the detail page
        public IActionResult Detail(int id)
        {
            // Find the single record
            ViewBag.Single = repo.Accidents
             .Single(x => x.crash_id == id);

            return View();
        }
    }
}
