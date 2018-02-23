﻿using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EsMo.Android.Support.Views;
using EsMo.Sina.SDK;
using EsMo.Sina.SDK.Model;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using System;
using System.IO;
using UniversalImageLoader.Core;
using UniversalImageLoader.Core.Listener;

namespace EsMo.Android.WeiBo.Entity
{
    public class TimelineAdapter : MvxAdapter
    {
        public TimelineAdapter(Context context) : base(context)
        {
        }

        public TimelineAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
        }
        public TimelineAdapter(Context context, IMvxAndroidBindingContext bindingContext, int templateID) : base(context, bindingContext)
        {
            this.ItemTemplateId = templateID;
        }
        protected TimelineAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
        protected override IMvxListItemView CreateBindableView(object dataContext, ViewGroup parent, int templateId)
        {
            return new TimelineListItemView(this.Context, this.BindingContext.LayoutInflaterHolder, dataContext, parent, templateId);
        }
        protected override View GetBindableView(View convertView, object dataContext, ViewGroup parent, int templateId)
        {
            var view = base.GetBindableView(convertView, dataContext, parent, templateId);
            (view.Tag as TimelineListItemView).UpdateView(dataContext);
            return view;
        }
    }
    public class TimelineListItemView : MvxListItemView
    {
        WrappedLayout wrappedLayout;
        Drawable imgLoadingDrawable;
        public TimelineListItemView(Context context, IMvxLayoutInflaterHolder layoutInflaterHolder, object dataContext, ViewGroup parent, int templateId) : base(context, layoutInflaterHolder, dataContext, parent, templateId)
        {
            View content = this.Content;
            this.imgLoadingDrawable = new BitmapDrawable(ResourceExtension.ImageLoading);
            this.wrappedLayout = content.FindViewById<WrappedLayout>(Resource.Id.wrapPics);
            for (int i = 0; i < this.wrappedLayout.ChildCount; i++)
            {
                ImageView imgView = this.wrappedLayout.GetChildAt(i) as ImageView;
                imgView.SetScaleType(ImageView.ScaleType.CenterCrop);
                imgView.SetImageDrawable(this.imgLoadingDrawable);
            }
        }
        public void UpdateView(object dataContext)
        {
            var model = dataContext as TimelineItemViewModel;
            for (int i = 0; i < this.wrappedLayout.ChildCount; i++)
            {
                var imgView = this.wrappedLayout.GetChildAt(i) as ImageView;
                imgView.SetImageDrawable(this.imgLoadingDrawable);
            }

            for (int i = 0; i < this.wrappedLayout.ChildCount; i++)
            {
                if (i < model.ImageModels.Count)
                {
                    var imgView = this.wrappedLayout.GetChildAt(i) as ImageView;
                    var imgModel = model.ImageModels[i];
                    ImageLoader.Instance.LoadImage(imgModel.ThumbnailPic, new EsMoImageLoadingListener(imgView));
                }
            }
        }
    }
    public class EsMoImageLoadingListener : SimpleImageLoadingListener
    {
        ImageView imgView;
        public EsMoImageLoadingListener(ImageView imageView)
        {
            this.imgView = imageView;
        }

        protected EsMoImageLoadingListener(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }
        public override void OnLoadingComplete(string imageUri, View view, Bitmap loadedImage)
        {
            base.OnLoadingComplete(imageUri, view, loadedImage);
            this.ApplyImage(loadedImage);
        }
        private void ApplyImage(Bitmap bitmap)
        {
            BitmapDrawable bmp = new BitmapDrawable(bitmap);
            Drawable targetDrawable = null;
            if (imgView.Drawable != null)
            {
                TransitionDrawable trans = new TransitionDrawable(new Drawable[] { imgView.Drawable, bmp });
                trans.StartTransition(300);
                targetDrawable = trans;
            }
            else
                targetDrawable = bmp;
            imgView.SetImageDrawable(targetDrawable);
        }
    }
}
