
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Template10.Services;
using Template10.Services.Serialization.Tests.Models;

namespace Template10.Services.Serialization.Tests
{
    [TestClass]
    public class JsonSerializationServiceTests
    {
        [TestMethod]
        public void Json_Initialize()
        {
            var ser = new JsonSerializationService();
            Assert.IsNotNull(ser);
        }

        [TestMethod]
        public void Json_SerializeModel_Succeed()
        {
            var now = DateTime.Now;
            var model = ReminderFactory.Create(now);

            Assert.AreEqual(now, model.NextDue);

            var ser = new JsonSerializationService();

            var result = ser.Serialize(model);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Text.Json.JsonException))]
        public void Json_SerializeModel_Fail()
        {
            var ser = new JsonSerializationService();
            var recurse = new RecurseModel();
            recurse.ToInfinity = recurse;

            var result = ser.Serialize(recurse);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Json_DeserializeModel()
        {
            var json = "{\"TimeRange\":2,\"DayRange\":3,\"CustomDays\":1,\"Title\":\"Test Reminder\"," +
                "\"Notes\":\"Exercise full range of motion\",\"HowManyTimesADay\":4," +
                "\"StartDate\":\"2020-02-11T13:43:34.9119383-07:00\",\"EndDate\":\"2020-03-03T13:43:34.9119383-07:00\","+
                "\"NextDue\":\"2020-02-18T13:43:34.9119383-07:00\"}";

            var ser = new JsonSerializationService();

            var result = ser.Deserialize<Reminder>(json);
            Assert.IsInstanceOfType(result, typeof(Reminder));
        }

        [TestMethod]
        [ExpectedException(typeof(System.Text.Json.JsonException))]
        public void Json_DeserializeModel_Fail()
        {
            var json = "{INVALID\"TimeRange\":2,\"DayRange\":3,\"CustomDays\":1,\"Title\":\"Test Reminder\"," +
                "\"Notes\":\"Exercise full range of motion\",\"HowManyTimesADay\":4," +
                "\"StartDate\":\"2020-02-11T13:43:34.9119383-07:00\",\"EndDate\":\"2020-03-03T13:43:34.9119383-07:00\"," +
                "\"NextDue\":\"2020-02-18T13:43:34.9119383-07:00\"}";

            var ser = new JsonSerializationService();

            var result = ser.Deserialize<Reminder>(json);
            Assert.IsInstanceOfType(result, typeof(Reminder));
        }

        [TestMethod]
        public void Json_TryDeserializeModel_Fail()
        {
            var json = "{INVALID\"TimeRange\":2,\"DayRange\":3,\"CustomDays\":1,\"Title\":\"Test Reminder\"," +
                "\"Notes\":\"Exercise full range of motion\",\"HowManyTimesADay\":4," +
                "\"StartDate\":\"2020-02-11T13:43:34.9119383-07:00\",\"EndDate\":\"2020-03-03T13:43:34.9119383-07:00\"," +
                "\"NextDue\":\"2020-02-18T13:43:34.9119383-07:00\"}";

            var ser = new JsonSerializationService();

            Reminder resultReminder;
            var result = ser.TryDeserialize<Reminder>(json, out resultReminder);
            Assert.IsFalse(result);
            Assert.IsNull(resultReminder);
        }


        [TestMethod]
        public void Json_TryDeserializeModel_Succeed()
        {
            var json = "{\"TimeRange\":2,\"DayRange\":3,\"CustomDays\":1,\"Title\":\"Test Reminder\"," +
                "\"Notes\":\"Exercise full range of motion\",\"HowManyTimesADay\":4," +
                "\"StartDate\":\"2020-02-11T13:43:34.9119383-07:00\",\"EndDate\":\"2020-03-03T13:43:34.9119383-07:00\"," +
                "\"NextDue\":\"2020-02-18T13:43:34.9119383-07:00\"}";

            var ser = new JsonSerializationService();

            Reminder resultReminder;
            var result = ser.TryDeserialize<Reminder>(json, out resultReminder);
            Assert.IsTrue(result);
            Assert.IsNotNull(resultReminder);
            Assert.IsInstanceOfType(resultReminder, typeof(Reminder));
        }


        [TestMethod]
        public void Json_TrySerializeModel_Fail()
        {
            var ser = new JsonSerializationService();
            var recurse = new RecurseModel();
            recurse.ToInfinity = recurse;

            string serializerResult;
            var result = ser.TrySerialize(recurse, out serializerResult);
            Assert.IsFalse(result);
            Assert.IsNull(serializerResult);
        }


        [TestMethod]
        public void Json_TrySerializeModel_Succeed()
        {
            var now = DateTime.Now;
            var model = ReminderFactory.Create(now);
            var ser = new JsonSerializationService();

            string serializerResult;
            var result = ser.TrySerialize(model, out serializerResult);
            Assert.IsTrue(result);
            Assert.IsNotNull(serializerResult);
        }
    }
}
