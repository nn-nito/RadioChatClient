using Api;
using Common;
using System;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Demos.Common;
using Com.TheFallenGames.OSA.Demos.Common.SceneEntries;
using Com.TheFallenGames.OSA.Demos.Chat;

namespace Chat
{
    /// <summary>Hookup between the <see cref="Common.Drawer.DrawerCommandPanel"/> and the adapter to isolate example code from demo-ing and navigation code</summary>
    public class ChatSceneEntry : SceneEntry<ChatExample, MyParams, ChatMessageViewsHolder>
    {
        protected override void InitDrawer()
        {
            _Drawer.Init(_Adapters, false, false, false, false, true, false);
        }

        protected override void OnAllAdaptersInitialized()
        {
            base.OnAllAdaptersInitialized();

            // OnItemCountChangeRequested(_Adapters[0], 3);
        }

        #region events from DrawerCommandPanel
        protected override void OnAddItemRequested(ChatExample adapter, ChatModel.Chat chat, int index)
        {
            // base.OnAddItemRequested(adapter, index);

            var parse = DateTime.Parse(chat.time_sent);
            ChatMessageModel chatMessageModel = new ChatMessageModel()
            {
                dateTime = parse,
                Text = chat.message,
                UserName = chat.user_name,
                IsMine = true,
            };

            adapter.Data.InsertOne(index, chatMessageModel, true);
        }

        protected override void OnAddItemsRequested(ChatExample adapter, ChatModel.Chats chats, int index)
        {
            // base.OnAddItemRequested(adapter, index);

            List<ChatMessageModel> chatMessageModels = new List<ChatMessageModel>();
            foreach (var chat in chats.chats)
            {
                var parse = DateTime.Parse(chat.time_sent);
                ChatMessageModel chatMessageModel = new ChatMessageModel()
                {
                    dateTime = parse,
                    Text = chat.message,
                    UserName = chat.user_name,
                    IsMine = (chat.user_id == Boot.Instance.UserId) ? true : false
                };
                chatMessageModels.Add(chatMessageModel);
            }

            adapter.Data.InsertItems(index, chatMessageModels, true);
        }

        protected override void OnItemCountChangeRequested(ChatExample adapter, int count)
        {
            base.OnItemCountChangeRequested(adapter, count);

            // Generating some random models
            var newModels = new ChatMessageModel[count];
            for (int i = 0; i < count; ++i)
                newModels[i] = CreateRandomModel(i, i != 1); // the second model will always have an image, for demo purposes

            adapter.Data.ResetItems(newModels, true);
        }
        #endregion

        ChatMessageModel CreateRandomModel(int itemIdex, bool addImageOnlyRandomly = true)
        {
            return new ChatMessageModel()
            {
                dateTime = DateTime.Now,
                Text = GetRandomContent(),
                UserName = "ぺこら",
                IsMine = UnityEngine.Random.Range(0, 2) == 0,
                // ImageIndex = addImageOnlyRandomly ?
                // 				// Twice as many messages without photo as with photo
                // 				UnityEngine.Random.Range(
                // 					-2 * _Adapters[0].Parameters.availableChatImages.Length,
                // 					_Adapters[0].Parameters.availableChatImages.Length
                // 				)
                // 				: 0
            };
        }

        string GetRandomContent() { return DemosUtil.GetRandomTextBody(0, UnityEngine.Random.Range(DemosUtil.LOREM_IPSUM.Length / 50 + 1, DemosUtil.LOREM_IPSUM.Length / 2)); }
    }
}
