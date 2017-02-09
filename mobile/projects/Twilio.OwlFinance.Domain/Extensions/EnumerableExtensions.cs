using System;
using System.Collections;
using System.Collections.Generic;
using Twilio.OwlFinance.Domain.Interfaces;

namespace Twilio.OwlFinance.Domain.Extensions
{
    public static class EnumerableExtensions
    {
        public static List<TOutput> ConvertAll<TConverter, TInput, TOutput>(this IEnumerable inputs)
            where TConverter : IConverter<TInput, TOutput>, new()
        {

            IConverter<TInput, TOutput> converter = (IConverter<TInput, TOutput>)Activator.CreateInstance(typeof(TConverter));
            List<TOutput> outputList = new List<TOutput>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (TInput input in inputs)
            {
                outputList.Add(converter.Convert(input));
            }

            return outputList;
        }

		public static TOutput ConvertOne<TConverter, TInput, TOutput>(this object input)
			where TConverter : IConverter<TInput, TOutput>, new()
		{

			IConverter<TInput, TOutput> converter = (IConverter<TInput, TOutput>)Activator.CreateInstance(typeof(TConverter));

			TOutput output = converter.Convert((TInput) input);

			return output;
		}
    }
}
