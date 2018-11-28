using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using HGGM.Models.Events;
using HGGM.Models.Identity;
using LiteDB;
using Xunit;

namespace HGGM.UnitTests.Models.Events
{
    public class EventTests
    {
        [Fact]
        public void EventIsLiteDbCompatible()
        {
            using (var mem = new MemoryStream())
            using (var db = new LiteRepository(mem))
            {
                var user = new User {UserName = "asd"};
                var e = new Event
                {
                    Author = user,
                    Publisher = user,
                    Roster =
                        new List<Slot>
                        {
                            new Slot
                            {
                                SubSlots = new List<Slot>
                                {
                                    new Slot
                                    {
                                        SignUps = new List<SlotSignUp>
                                        {
                                            new SlotSignUp
                                            {
                                                User = user
                                            }
                                        }
                                    }
                                },
                                SignUps = new List<SlotSignUp>
                                {
                                    new SlotSignUp
                                    {
                                        User = user
                                    }
                                }
                            }
                        },
                    TakesPlace = DateTimeOffset.Now
                };
                db.Insert(user);
                db.Insert(e);
                var o = db.Query<Event>().Single();
                o.Author.Id.Should().Be(user.Id);
                o.Publisher.Id.Should().Be(user.Id);
                o.Roster.Should().NotBeNull().And.HaveCount(1).And.NotContainNulls();
                o.Roster.Single().SubSlots.Should().NotBeNull().And.HaveCount(1).And.NotContainNulls();
                o.Roster.Single().SubSlots.Single().SignUps.Should().NotBeNull().And.HaveCount(1).And.NotContainNulls();
                o.Roster.Single().SubSlots.Single().SignUps.Single().User.Id.Should().Be(user.Id);
                o.Roster.Single().SignUps.Should().NotBeNull().And.HaveCount(1).And.NotContainNulls();
                o.Roster.Single().SignUps.Single().User.Id.Should().Be(user.Id);
            }
        }
    }
}