﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Portal.Web.Models.Teachers;
using OtripleS.Portal.Web.Models.Teachers.Exceptions;
using RESTFulSense.Exceptions;

namespace OtripleS.Portal.Web.Services.Teachers
{
    public partial class TeacherService
    {
        private delegate ValueTask<IList<Teacher>> ReturningStudentFunction();

        private async ValueTask<IList<Teacher>> TryCatch(ReturningStudentFunction returningStudentFunction)
        {
            try
            {
                return await returningStudentFunction();
            }
            catch (HttpResponseInternalServerErrorException httpResponseInternalServerException)
            {
                throw CreateAndLogDependencyException(httpResponseInternalServerException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private TeacherDependencyValidationException CreateAndLogDependencyException(
            Exception exception)
        {
            var teacherDependencyException =
                new TeacherDependencyValidationException(exception);

            this.loggingBroker.LogError(teacherDependencyException);

            return teacherDependencyException;
        }

        private TeacherServiceException CreateAndLogServiceException(
            Exception exception)
        {
            var teacherServiceException =
                new TeacherServiceException(exception);

            this.loggingBroker.LogError(teacherServiceException);

            return teacherServiceException;
        }
    }
}
