using System;
using System.Collections.Generic;
using System.Text;

namespace EntregasLogyTechSharedModel.FineTune
{
    public class FineTuneResponseModel
    {
        public class Hyperparams
        {
            public int n_epochs { get; set; }
            public object batch_size { get; set; }
            public double prompt_loss_weight { get; set; }
            public object learning_rate_multiplier { get; set; }
        }

        public class TrainingFile
        {
            public string @object { get; set; }
            public string id { get; set; }
            public string purpose { get; set; }
            public string filename { get; set; }
            public int bytes { get; set; }
            public int created_at { get; set; }
            public string status { get; set; }
            public object status_details { get; set; }
        }

        public class Event
        {
            public string @object { get; set; }
            public string level { get; set; }
            public string message { get; set; }
            public int created_at { get; set; }
        }

        public class Root
        {
            public string @object { get; set; }
            public string id { get; set; }
            public Hyperparams hyperparams { get; set; }
            public string organization_id { get; set; }
            public string model { get; set; }
            public TrainingFile[] training_files { get; set; }
            public object[] validation_files { get; set; }
            public object[] result_files { get; set; }
            public int created_at { get; set; }
            public int updated_at { get; set; }
            public string status { get; set; }
            public object fine_tuned_model { get; set; }
            public Event[] events { get; set; }
        }
    }
}
