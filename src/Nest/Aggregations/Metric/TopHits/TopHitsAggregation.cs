﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	[JsonConverter(typeof(ReadAsTypeJsonConverter<TopHitsAggregation>))]
	public interface ITopHitsAggregation : IMetricAggregation
	{
		[JsonProperty("from")]
		int? From { get; set; }

		[JsonProperty("size")]
		int? Size { get; set; }

		[JsonProperty("sort")]
		[JsonConverter(typeof(SortCollectionJsonConverter))]
		IList<ISort> Sort { get; set; }

		[JsonProperty("_source")]
		ISourceFilter Source { get; set; }

		[JsonProperty("highlight")]
		IHighlight Highlight { get; set; }

		[JsonProperty("explain")]
		bool? Explain { get; set; }

		[JsonProperty("script_fields")]
		[JsonConverter(typeof(ReadAsTypeJsonConverter<ScriptFields>))]
		IScriptFields ScriptFields { get; set; }

		[JsonProperty("fielddata_fields")]
		IEnumerable<Field> FielddataFields { get; set; }

		[JsonProperty("version")]
		bool? Version { get; set; }
	}

	public class TopHitsAggregation : MetricAggregationBase, ITopHitsAggregation
	{
		public int? From { get; set; }
		public int? Size { get; set; }
		public IList<ISort> Sort { get; set; }
		public ISourceFilter Source { get; set; }
		public IHighlight Highlight { get; set; }
		public bool? Explain { get; set; }
		public IScriptFields ScriptFields { get; set; }
		public IEnumerable<Field> FielddataFields { get; set; }
		public bool? Version { get; set; }

		internal TopHitsAggregation() { }

		public TopHitsAggregation(string name) : base(name, null) { }

		internal override void WrapInContainer(AggregationContainer c) => c.TopHits = this;
	}

	public class TopHitsAggregationDescriptor<T>
		: MetricAggregationDescriptorBase<TopHitsAggregationDescriptor<T>, ITopHitsAggregation, T>
			, ITopHitsAggregation
		where T : class
	{
		int? ITopHitsAggregation.From { get; set; }

		int? ITopHitsAggregation.Size { get; set; }

		IList<ISort> ITopHitsAggregation.Sort { get; set; }

		ISourceFilter ITopHitsAggregation.Source { get; set; }

		IHighlight ITopHitsAggregation.Highlight { get; set; }

		bool? ITopHitsAggregation.Explain { get; set; }

		IScriptFields ITopHitsAggregation.ScriptFields { get; set; }

		IEnumerable<Field> ITopHitsAggregation.FielddataFields { get; set; }

		bool? ITopHitsAggregation.Version { get; set; }

		public TopHitsAggregationDescriptor<T> From(int from) => Assign(a => a.From = from);

		public TopHitsAggregationDescriptor<T> Size(int size) => Assign(a => a.Size = size);

		public TopHitsAggregationDescriptor<T> Sort(Func<SortFieldDescriptor<T>, IFieldSort> sortSelector) => Assign(a =>
		{
			a.Sort = a.Sort ?? new List<ISort>();
			var sort = sortSelector?.Invoke(new SortFieldDescriptor<T>());
            if (sort != null) a.Sort.Add(sort);		
		});

		public TopHitsAggregationDescriptor<T> Source(bool include = true) =>
			Assign(a => a.Source = !include ? SourceFilter.ExcludeAll : null);

		public TopHitsAggregationDescriptor<T> Source(Func<SearchSourceDescriptor<T>, SearchSourceDescriptor<T>> sourceSelector) =>
			Assign(a => a.Source = sourceSelector?.Invoke(new SearchSourceDescriptor<T>()));

		public TopHitsAggregationDescriptor<T> Highlight(Func<HighlightDescriptor<T>, HighlightDescriptor<T>> highlightSelector) =>
			Assign(a => a.Highlight = highlightSelector?.Invoke(new HighlightDescriptor<T>()));

		public TopHitsAggregationDescriptor<T> Explain(bool explain = true) => Assign(a => a.Explain = explain);

		public TopHitsAggregationDescriptor<T> ScriptFields(Func<ScriptFieldsDescriptor, IPromise<IScriptFields>> scriptFieldsSelector) =>
			Assign(a => a.ScriptFields = scriptFieldsSelector?.Invoke(new ScriptFieldsDescriptor())?.Value);

		public TopHitsAggregationDescriptor<T> FielddataFields(params Field[] fields) =>
			Assign(a => a.FielddataFields = fields);

		public TopHitsAggregationDescriptor<T> FielddataFields(params Expression<Func<T, object>>[] objectPaths) =>
			Assign(a => a.FielddataFields = objectPaths?.Select(e => (Field) e).ToListOrNullIfEmpty());

		public TopHitsAggregationDescriptor<T> Version(bool version = true) => Assign(a => a.Version = version);
	}
}
