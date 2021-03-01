using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using BettingRoulette.Common.GenericClass;
using BettingRoulette.Model.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Infrastructure.Services
{
    public class LogRegisterService : ILogRegisterService
    {
        SecretSettings _settings;
        private readonly AmazonCloudWatchLogsClient _amazonCWL;

        public LogRegisterService(SecretSettings settings)
        {
            _settings = settings;
            _amazonCWL =
              new AmazonCloudWatchLogsClient(
                _settings.AccessKey,
                 _settings.SecretAccessKey,
                  Amazon.RegionEndpoint.GetBySystemName(_settings.RegionEndpoint)
                  );
        }

        public async Task<bool> RegisterLog(RegisterModel model)
        {
            model.LogGroupName = _settings.LogGroupName;
            bool createstream = false;
            try
            {
                var describeLogGroupsRequest = new DescribeLogGroupsRequest()
                {
                    LogGroupNamePrefix = model.LogGroupName
                };
                var groups = await _amazonCWL.DescribeLogGroupsAsync(describeLogGroupsRequest);
                if (groups.LogGroups.Count().Equals(default(int)))
                {
                    await _amazonCWL.CreateLogGroupAsync(new CreateLogGroupRequest
                    {
                        LogGroupName = model.LogGroupName
                    });
                }
                var describeLogStreamsRequest = new DescribeLogStreamsRequest
                {
                    LogGroupName = model.LogGroupName
                };
                var streams = await _amazonCWL.DescribeLogStreamsAsync(describeLogStreamsRequest);
                if (!(streams.LogStreams.Any(a => a.LogStreamName.ToUpper().Equals(model.LogStreamName.ToUpper()))))
                {
                    await _amazonCWL.CreateLogStreamAsync(new CreateLogStreamRequest
                    {
                        LogGroupName = model.LogGroupName,
                        LogStreamName = model.LogStreamName
                    });
                    createstream = true;
                }
                var errorMessage = $"Message: {model.ErrorMessage} DeveloperMessage: {model.Exception}";
                var putlogEventsRequest = new PutLogEventsRequest
                {
                    LogGroupName = model.LogGroupName,
                    LogStreamName = model.LogStreamName,
                    LogEvents = new List<InputLogEvent>
                    {
                        new InputLogEvent
                        {
                            Timestamp = DateTime.Now,
                            Message = errorMessage
                        }
                    }
                };
                if (!createstream)
                {
                    var uploadSequenceToken = streams.LogStreams.FirstOrDefault(w => w.LogStreamName.Equals(model.LogStreamName)).UploadSequenceToken;
                    putlogEventsRequest.SequenceToken = uploadSequenceToken;
                }
                await _amazonCWL.PutLogEventsAsync(putlogEventsRequest);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
