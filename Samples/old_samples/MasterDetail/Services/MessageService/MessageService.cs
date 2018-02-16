using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Template10.Samples.MasterDetailSample.Models;
using Template10.Utils;

namespace Template10.Samples.MasterDetailSample.Services.MessageService
{
    public partial class MessageService
    {
        private static ObservableCollection<Message> _messages;
        private readonly Random _random = new Random((int)DateTime.Now.Ticks);

        public ObservableCollection<Message> GetMessages()
        {
            if (_messages != null)
                return _messages;
            return _messages = _messages 
                ?? SampleMessages().ToObservableCollection();
        }

        IEnumerable<Models.Message> SampleMessages()
        {
            var froms = new[] {
                "Jonathan Archer",
                "T'Pol Main",
                "Charles 'Trip' Tucker III",
                "Malcolm Reed",
                "Hoshi Sato Main",
                "Travis Mayweather",
                "Phlox",
                "Thy'lek Shran",
                "Maxwell Forrest",
                "Matt Winston"
            };
            var subjects = new[] {
                "Oak is strong and also gives shade.",
                "Cats and dogs each hate the other.",
                "The pipe began to rust while new.",
                "Open the crate but don't break the glass.",
                "Add the sum to the product of these three.",
                "Thieves who rob friends deserve jail.",
                "The ripe taste of cheese improves with age.",
                "Act on these orders with great speed.",
                "The hog crawled under the high fence.",
                "Move the vat over the hot fire."
            };
            var bodys = new[] {
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam ullamcorper arcu nulla, vitae congue risus ullamcorper non. Maecenas id velit turpis. In vehicula vulputate ante non venenatis. Nam euismod, ex id congue laoreet, massa libero commodo nulla, pellentesque dictum leo sem feugiat lacus. Nulla eu mi eu purus convallis rhoncus vel quis quam. Quisque quis odio non purus interdum iaculis quis vitae mi. Aliquam dapibus sed ex ut posuere. Mauris commodo quam purus, quis dignissim arcu rhoncus eget. Donec lorem lorem, ornare at felis id, accumsan blandit ex. Donec hendrerit est velit, sed malesuada justo fermentum at. Nulla egestas lacus orci, sed tincidunt dolor congue vitae.",
                "Phasellus euismod massa massa, porta dictum ipsum pulvinar et. Vestibulum mollis nisi id neque maximus porta sed tincidunt ligula. Proin mollis nulla eu ex consectetur, vel pretium ipsum gravida. Nam nec ipsum bibendum elit accumsan pretium. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Etiam pulvinar, neque nec aliquet luctus, diam lacus sollicitudin justo, sit amet maximus diam sem in urna. In at dolor dapibus libero hendrerit tincidunt in auctor tortor. Mauris imperdiet consequat nunc, vel auctor libero dignissim ut. Nullam eget orci sit amet dolor accumsan tincidunt accumsan sed turpis.",
                "Nullam eu nibh augue. Nullam accumsan lorem sit amet tortor placerat dignissim ut dapibus massa. Suspendisse ante tortor, aliquet eu ipsum et, rutrum pulvinar leo. In hac habitasse platea dictumst. Suspendisse sit amet efficitur dolor. Donec orci massa, consectetur non est sed, tempus dictum libero. Morbi porttitor consectetur magna mollis iaculis. Cras eros libero, pellentesque ac rhoncus non, convallis sit amet ipsum. Quisque mattis vitae libero vitae laoreet. Aenean cursus ullamcorper lectus sit amet vehicula. Vivamus id ligula et risus laoreet vehicula. Fusce purus dolor, mollis a felis id, fringilla aliquet elit. Morbi porta lorem accumsan augue laoreet aliquam vel id ex. Curabitur sodales facilisis ligula non dictum. Etiam pharetra, elit at aliquam pellentesque, sapien justo congue eros, sed porta orci augue eget velit.",
                "Duis auctor, dolor ut aliquet commodo, ligula quam malesuada leo, in aliquet nunc massa ac lorem. Quisque volutpat ultrices lacus a imperdiet. Sed nisi sem, aliquet ut gravida a, fermentum sit amet massa. Praesent tincidunt sapien ac nisi pellentesque mattis quis ac lorem. Phasellus iaculis, dui sed dictum dapibus, ipsum dolor volutpat tellus, sed ultricies arcu eros in dui. Suspendisse blandit congue venenatis. Mauris quis mauris lectus. Pellentesque at massa bibendum, mattis urna non, vulputate tortor. Pellentesque pellentesque semper lectus a porttitor.",
                "Mauris volutpat diam lacus, id imperdiet orci tempus id. Aenean mollis non turpis sed lacinia. Nullam nisi mauris, rutrum sit amet libero a, aliquam pretium est. Vivamus a justo at dolor convallis accumsan. Donec eget pellentesque elit. Ut ullamcorper et libero nec pretium. Fusce lacinia enim orci, sed accumsan justo blandit id. Morbi magna turpis, placerat eget vehicula quis, tristique non metus. Donec efficitur pellentesque egestas. Fusce at nunc eros. Morbi finibus velit vitae dui dictum, eget scelerisque ante tincidunt.",
                "Praesent gravida tortor id ipsum vulputate tempus. Phasellus posuere arcu eget felis suscipit, sed condimentum tortor imperdiet. Duis quam turpis, iaculis a odio eget, auctor aliquet dui. Duis aliquet cursus rhoncus. Aliquam aliquet ex id rhoncus mattis. Vivamus in sagittis nibh, eu aliquam purus. Phasellus vel leo in est imperdiet mattis vitae a orci. Nullam risus purus, imperdiet at nisl at, sollicitudin ullamcorper enim. Duis eleifend nisi nibh, eget ultrices elit ullamcorper ac. Morbi leo justo, tempus sit amet lorem et, blandit sodales diam. Morbi viverra leo quis lacus tempus posuere. Ut sed orci augue.",
                "Curabitur porttitor nunc libero, vel sodales enim viverra consectetur. Duis interdum ligula ex, id eleifend urna egestas in. Duis pellentesque egestas odio, nec consequat orci mollis efficitur. Nulla quis diam pellentesque, semper ante eget, hendrerit nibh. Quisque venenatis, nibh non facilisis convallis, diam orci aliquet sem, vitae cursus nulla nunc sed quam. Praesent sit amet magna at velit posuere ornare. In sagittis condimentum ligula, sed porttitor felis vestibulum nec. Nullam a malesuada ante. Aliquam suscipit sed orci quis accumsan. Aenean rhoncus libero libero, eu aliquam elit vehicula id. Maecenas et enim at velit luctus efficitur. Praesent a nulla lorem. Donec non turpis sed urna volutpat scelerisque. Mauris tristique dolor eget libero placerat rutrum non non massa. Cras sagittis, libero sed tempor congue, justo est vehicula magna, tincidunt fringilla metus tellus ut enim.",
                "Proin venenatis ipsum neque, at vehicula risus ullamcorper lacinia. Ut justo justo, placerat in tempor at, tristique id enim. Sed ut diam placerat nibh tincidunt consequat. In non volutpat erat. Maecenas nec purus hendrerit, accumsan mi quis, porttitor nibh. Donec ultrices tempor hendrerit. Vivamus dictum erat eleifend, lobortis justo vel, fermentum risus. Nam vitae nibh odio. Donec lacinia rhoncus augue nec mollis. Curabitur a sapien nisl.",
                "Aliquam sit amet mi a lacus vehicula iaculis. Curabitur lacinia commodo tellus, eget varius nisl feugiat non. Vivamus sit amet sem venenatis, sollicitudin dolor ultricies, mattis felis. Nulla imperdiet lacinia varius. Nullam ut mauris tortor. Nam ac congue mauris. Pellentesque et pretium leo. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Cras fermentum felis nibh, vitae pretium velit sodales vel. Cras blandit arcu et porta vehicula. Fusce pulvinar ligula mi, eget vestibulum turpis tincidunt et. Morbi facilisis facilisis felis, eu interdum felis tempor vel. Praesent in turpis porta erat aliquet ultricies interdum sit amet massa.",
                "XDonec rutrum magna a turpis pharetra, sit amet commodo sapien consequat. Duis dictum ante nunc, eu sollicitudin lacus imperdiet quis. Donec vel est eget ligula mollis venenatis ut quis metus. Vivamus gravida, ligula vitae rhoncus mattis, magna est venenatis lectus, ut viverra dui est et massa.Quisque faucibus a diam at porta. Nam sit amet consequat dui.Ut elementum leo orci, eget dictum diam rutrum eget. Aliquam ac mattis libero. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aenean mattis enim vel tincidunt fermentum. Fusce in imperdiet ipsum. "
            };
            for (int i = 0; i < 10; i++)
            {
                var message = new Models.Message
                {
                    Id = Guid.NewGuid().ToString(),
                    Date = DateTime.Now.Subtract(TimeSpan.FromDays(_random.Next(0, 10))),
                    Subject = subjects[i],
                    Body = bodys[i],
                    From = froms[i],
                    To = "Jerry Nixon"
                };
                yield return message;
            }
        }
    }
}