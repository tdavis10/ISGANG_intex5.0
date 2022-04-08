﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
// Authors Jacob Poor, Ryan Pinkney, Kevin Gutierrez, Tanner Davis
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthLab2_RyanPinkney.Controllers
{
    public class InferenceController : Controller
    {
        // creates an instance of the Inference Session
        private InferenceSession _session;

        public InferenceController(InferenceSession session)
        {
            _session = session;
        }

        // calls the action and page to score the model
        [HttpPost]
        public ActionResult Score(CrashData data)
        {
            var result = _session.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor())
            });
            Tensor<string> score = result.First().AsTensor<string>();
            var prediction = new Prediction { PredictedValue = score.Last()};
            result.Dispose();

            return View("Result", prediction);
        }

        // First route for the result
        public IActionResult Result()
        {
            return View();
        }



    }
}
