﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Composing;
using YuzuDelivery.Core;

namespace YuzuDelivery.Umbraco.PositionalContent
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class YuzuPosContartup : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<PositionalContentParseService>();

            AddDefaultPosConContentItems(composition);
            AddDefaultPosConImageItems(composition);

        }

        public void AddDefaultPosConContentItems(Composition composition)
        {
            composition.Register<IPosConContentItemInternal[]>((factory) =>
            {
                var config = factory.GetInstance<IYuzuConfiguration>();
                var mapper = factory.GetInstance<IMapper>();

                var basePosConItem = typeof(DefaultPosConContentItem<,>);
                var posConItems = new List<IPosConContentItemInternal>();
                var viewmodelTypes = config.ViewModels.Where(x => x.Name.StartsWith(YuzuConstants.Configuration.BlockPrefix));

                foreach (var viewModelType in viewmodelTypes)
                {
                    var umbracoModelTypeName = viewModelType.Name.Replace(YuzuConstants.Configuration.BlockPrefix, "");
                    var alias = umbracoModelTypeName.FirstCharacterToLower();
                    var umbracoModelType = config.CMSModels.Where(x => x.Name == umbracoModelTypeName).FirstOrDefault();

                    if (umbracoModelType != null && umbracoModelType.BaseType == typeof(PublishedElementModel))
                    {
                        var makeme = basePosConItem.MakeGenericType(new Type[] { umbracoModelType, viewModelType });
                        var o = Activator.CreateInstance(makeme, new object[] { mapper }) as IPosConContentItemInternal;

                        posConItems.Add(o);
                    }
                }

                return posConItems.ToArray();
            }, Lifetime.Singleton);
        }

        public void AddDefaultPosConImageItems(Composition composition)
        {
            composition.Register<IPosConImageItemInternal[]>((factory) =>
            {
                var config = factory.GetInstance<IYuzuConfiguration>();
                var mapper = factory.GetInstance<IMapper>();

                var basePosConItem = typeof(DefaultPosConImagetem<,>);
                var posConItems = new List<IPosConImageItemInternal>();
                var viewmodelTypes = config.ViewModels.Where(x => x.Name.StartsWith(YuzuConstants.Configuration.BlockPrefix));

                foreach (var viewModelType in viewmodelTypes)
                {
                    var umbracoModelTypeName = viewModelType.Name.Replace(YuzuConstants.Configuration.BlockPrefix, "");
                    var alias = umbracoModelTypeName.FirstCharacterToLower();
                    var umbracoModelType = config.CMSModels.Where(x => x.Name == umbracoModelTypeName).FirstOrDefault();

                    if (umbracoModelType != null && umbracoModelType.BaseType == typeof(PublishedElementModel))
                    {
                        var makeme = basePosConItem.MakeGenericType(new Type[] { umbracoModelType, viewModelType });
                        var o = Activator.CreateInstance(makeme, new object[] { mapper }) as IPosConImageItemInternal;

                        posConItems.Add(o);
                    }
                }

                return posConItems.ToArray();
            }, Lifetime.Singleton);
        }
    }
}
