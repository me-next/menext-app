using System;
using System.Collections.Generic;

namespace MeNext.MusicService
{
	/// <summary>
	/// Represents a page of results for something.
	/// </summary>
	public interface IResultList<Type>
	{
		/// <summary>
		/// Gets the type of error (if any) for this request.
		/// If this is not SUCCESS, generally one shouldn't do anything with the rest of this object.
		/// </summary>
		/// <value>The error.</value>
		PageErrorType Error
		{
			get;
		}

		/// <summary>
		/// Gets the list of items in this page.
		/// </summary>
		/// <value>The items.</value>
		List<Type> Items
		{
			get;
		}

		/// <summary>
		/// Gets the next page of results.
		/// </summary>
		/// <value>The next page.</value>
		IResultList<Type> NextPage
		{
			get;
		}

		/// <summary>
		/// Gets the previous page of results.
		/// </summary>
		/// <value>The previous page.</value>
		IResultList<Type> PreviousPage
		{
			get;
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:com.danielcentore.MusicService.IResultList`1"/> has a
		/// next page.
		/// </summary>
		/// <value><c>true</c> if has next page; otherwise, <c>false</c>.</value>
		bool HasNextPage
		{
			get;
		}

		/// <summary>
		/// Gets the index of the first item in this page.
		/// </summary>
		/// <value>The first result.</value>
		int FirstResult
		{
			get;
		}

		/// <summary>
		/// Gets the index of the last item in this page.
		/// </summary>
		/// <value>The last result.</value>
		int LastResult
		{
			get;
		}
	}
}
