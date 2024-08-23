#region Copyright

//  Stellaris Mod Manager used to manage a library of installed mods for the game of Stellaris
// Copyright (C) 2023  Matthew David van der Hoorn
//
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, at version 3 of the license.
//
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//     You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//
// CONTACT:
// Email: md.vanderhoorn@gmail.com
//     Business Email: admin@studyinstitute.net
// Discord: mr.hoornasp.learningexpert
// Phone: +31 6 18206979

#endregion

namespace MD.Common;

/// <summary>
/// Defines a bi-directional converter that converts objects of type T to objects of type T1, and vice versa.
/// </summary>
/// <typeparam name="T">The type of objects to convert from, and to convert back to.</typeparam>
/// <typeparam name="T1">The type of objects to convert to, and to convert back from.</typeparam>
public interface IConverterBi<T, T1> : IConverter<T, T1>
{
    /// <summary>
    /// Converts an object of type T1 back to an object of type T.
    /// </summary>
    /// <param name="toConvertBack">The object to convert back.</param>
    /// <returns>A new object of type T that is the result of the conversion back.</returns>
    T ConvertBack(T1 toConvertBack);
}