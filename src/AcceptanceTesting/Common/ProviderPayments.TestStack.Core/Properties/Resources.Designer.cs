﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProviderPayments.TestStack.Core.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ProviderPayments.TestStack.Core.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;Message xmlns=&quot;SFA/ILR/2016-17&quot; xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot;&gt;
        ///	&lt;Header&gt;
        ///		&lt;CollectionDetails&gt;
        ///			&lt;Collection&gt;ILR&lt;/Collection&gt;
        ///			&lt;Year&gt;1617&lt;/Year&gt;
        ///			&lt;FilePreparationDate&gt;2017-05-05&lt;/FilePreparationDate&gt;
        ///		&lt;/CollectionDetails&gt;
        ///		&lt;Source&gt;
        ///			&lt;ProtectiveMarking&gt;OFFICIAL-SENSITIVE-Personal&lt;/ProtectiveMarking&gt;
        ///			&lt;UKPRN&gt;10007459&lt;/UKPRN&gt;
        ///			&lt;SoftwareSupplier&gt;Software Supplier&lt;/SoftwareSupplier&gt;
        ///			&lt;SoftwarePackage&gt;Software Package&lt;/SoftwarePackage&gt;
        ///			&lt;Release&gt;Release&lt;/Relea [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string BaseIlr_1617 {
            get {
                return ResourceManager.GetString("BaseIlr_1617", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;Message xmlns=&quot;SFA/ILR/2017-18&quot; xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot;&gt;
        ///	&lt;Header&gt;
        ///		&lt;CollectionDetails&gt;
        ///			&lt;Collection&gt;ILR&lt;/Collection&gt;
        ///			&lt;Year&gt;1718&lt;/Year&gt;
        ///			&lt;FilePreparationDate&gt;2017-05-05&lt;/FilePreparationDate&gt;
        ///		&lt;/CollectionDetails&gt;
        ///		&lt;Source&gt;
        ///			&lt;ProtectiveMarking&gt;OFFICIAL-SENSITIVE-Personal&lt;/ProtectiveMarking&gt;
        ///			&lt;UKPRN&gt;10007459&lt;/UKPRN&gt;
        ///			&lt;SoftwareSupplier&gt;Software Supplier&lt;/SoftwareSupplier&gt;
        ///			&lt;SoftwarePackage&gt;Software Package&lt;/SoftwarePackage&gt;
        ///			&lt;Release&gt;Release&lt;/Relea [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string BaseILR_1718 {
            get {
                return ResourceManager.GetString("BaseILR_1718", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;Message xmlns=&quot;SFA/ILR/2017-18&quot; xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot;&gt;
        ///	&lt;Header&gt;
        ///		&lt;CollectionDetails&gt;
        ///			&lt;Collection&gt;ILR&lt;/Collection&gt;
        ///			&lt;Year&gt;1718&lt;/Year&gt;
        ///			&lt;FilePreparationDate&gt;2017-05-05&lt;/FilePreparationDate&gt;
        ///		&lt;/CollectionDetails&gt;
        ///		&lt;Source&gt;
        ///			&lt;ProtectiveMarking&gt;OFFICIAL-SENSITIVE-Personal&lt;/ProtectiveMarking&gt;
        ///			&lt;UKPRN&gt;10007459&lt;/UKPRN&gt;
        ///			&lt;SoftwareSupplier&gt;Software Supplier&lt;/SoftwareSupplier&gt;
        ///			&lt;SoftwarePackage&gt;Software Package&lt;/SoftwarePackage&gt;
        ///			&lt;Release&gt;Release&lt;/Relea [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string BaseILR_1819 {
            get {
                return ResourceManager.GetString("BaseILR_1819", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- Clean Deds
        ///DELETE FROM ${ILR_Deds.FQ}.Valid.LearningProvider
        ///WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
        ///GO
        ///
        ///DELETE FROM ${ILR_Deds.FQ}.Valid.TrailblazerApprenticeshipFinancialRecord
        ///WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
        ///GO
        ///
        ///DELETE FROM ${ILR_Deds.FQ}.Valid.LearningDeliveryFAM
        ///WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
        ///GO
        ///
        ///DELETE FROM ${ILR_Deds.FQ}.Valid.LearningDelivery
        ///WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CopyIlrDataToDeds_1617 {
            get {
                return ResourceManager.GetString("CopyIlrDataToDeds_1617", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- Clean Deds
        ///DELETE FROM ${ILR_Deds.FQ}.Valid.LearningProvider
        ///WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
        ///GO
        ///
        ///
        ///DELETE FROM ${ILR_Deds.FQ}.Valid.AppFinRecord
        ///WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
        ///GO
        ///
        ///DELETE FROM ${ILR_Deds.FQ}.Valid.LearningDeliveryFAM
        ///WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
        ///GO
        ///
        ///DELETE FROM ${ILR_Deds.FQ}.Valid.LearningDelivery
        ///WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
        ///GO
        ///
        ///DELETE FROM ${IL [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CopyIlrDataToDeds_1718 {
            get {
                return ResourceManager.GetString("CopyIlrDataToDeds_1718", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- Clean Deds
        ///DELETE FROM ${ILR_Deds.FQ}.Valid.LearningProvider
        ///WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
        ///GO
        ///
        ///
        ///DELETE FROM ${ILR_Deds.FQ}.Valid.AppFinRecord
        ///WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
        ///GO
        ///
        ///DELETE FROM ${ILR_Deds.FQ}.Valid.LearningDeliveryFAM
        ///WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
        ///GO
        ///
        ///DELETE FROM ${ILR_Deds.FQ}.Valid.LearningDelivery
        ///WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
        ///GO
        ///
        ///DELETE FROM ${IL [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CopyIlrDataToDeds_1819 {
            get {
                return ResourceManager.GetString("CopyIlrDataToDeds_1819", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /****** Object:  View [dbo].[ValidLearners]    Script Date: 12/09/2016 09:23:09 ******/
        ///IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N&apos;[dbo].[ValidLearners]&apos;))
        ///DROP VIEW [dbo].[ValidLearners]
        ///GO
        ////****** Object:  StoredProcedure [dbo].[TransformInputToValid_TrailblazerApprenticeshipFinancialRecord]    Script Date: 12/09/2016 09:23:09 ******/
        ///IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N&apos;[dbo].[TransformInputToValid_TrailblazerApprenticeshipFinancialRecord]&apos;) AND  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CopyValidLearnerRecordsTaskScript {
            get {
                return ResourceManager.GetString("CopyValidLearnerRecordsTaskScript", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TRUNCATE TABLE [dbo].[EAS_Submission]
        ///TRUNCATE TABLE [dbo].[EAS_Submission_Values]
        ///TRUNCATE TABLE [ProviderAdjustments].[Payments]
        ///GO
        ///.
        /// </summary>
        internal static string EAS_PeriodEnd_Cleanup_Deds_DML {
            get {
                return ResourceManager.GetString("EAS_PeriodEnd_Cleanup_Deds_DML", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot;?&gt;
        ///&lt;ArrayOfTable&gt;
        ///  &lt;Table ElementName=&quot;Header&quot;&gt;
        ///    &lt;ChildTables&gt;
        ///      &lt;Table ElementName=&quot;CollectionDetails&quot; TableName=&quot;Input.CollectionDetails&quot;&gt;
        ///        &lt;Fields&gt;
        ///          &lt;Field Name=&quot;CollectionDetails_Id&quot; PrimaryKey=&quot;true&quot; Identity=&quot;true&quot; /&gt;
        ///          &lt;Field Name=&quot;Collection&quot; /&gt;
        ///          &lt;Field Name=&quot;Year&quot; /&gt;
        ///          &lt;Field Name=&quot;FilePreparationDate&quot; /&gt;
        ///        &lt;/Fields&gt;
        ///      &lt;/Table&gt;
        ///      &lt;Table ElementName=&quot;Source&quot; TableName=&quot;Input.Source&quot;&gt;
        ///        &lt;Fields&gt;
        ///     [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ILRTableMap_1617 {
            get {
                return ResourceManager.GetString("ILRTableMap_1617", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot;?&gt;
        ///&lt;ArrayOfTable&gt;
        ///  &lt;Table ElementName=&quot;Header&quot;&gt;
        ///    &lt;ChildTables&gt;
        ///      &lt;Table ElementName=&quot;CollectionDetails&quot; TableName=&quot;Input.CollectionDetails&quot;&gt;
        ///        &lt;Fields&gt;
        ///          &lt;Field Name=&quot;CollectionDetails_Id&quot; PrimaryKey=&quot;true&quot; Identity=&quot;true&quot; /&gt;
        ///          &lt;Field Name=&quot;Collection&quot; /&gt;
        ///          &lt;Field Name=&quot;Year&quot; /&gt;
        ///          &lt;Field Name=&quot;FilePreparationDate&quot; /&gt;
        ///        &lt;/Fields&gt;
        ///      &lt;/Table&gt;
        ///      &lt;Table ElementName=&quot;Source&quot; TableName=&quot;Input.Source&quot;&gt;
        ///        &lt;Fields&gt;
        ///     [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ILRTableMap_1718 {
            get {
                return ResourceManager.GetString("ILRTableMap_1718", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot;?&gt;
        ///&lt;ArrayOfTable&gt;
        ///  &lt;Table ElementName=&quot;Header&quot;&gt;
        ///    &lt;ChildTables&gt;
        ///      &lt;Table ElementName=&quot;CollectionDetails&quot; TableName=&quot;Input.CollectionDetails&quot;&gt;
        ///        &lt;Fields&gt;
        ///          &lt;Field Name=&quot;CollectionDetails_Id&quot; PrimaryKey=&quot;true&quot; Identity=&quot;true&quot; /&gt;
        ///          &lt;Field Name=&quot;Collection&quot; /&gt;
        ///          &lt;Field Name=&quot;Year&quot; /&gt;
        ///          &lt;Field Name=&quot;FilePreparationDate&quot; /&gt;
        ///        &lt;/Fields&gt;
        ///      &lt;/Table&gt;
        ///      &lt;Table ElementName=&quot;Source&quot; TableName=&quot;Input.Source&quot;&gt;
        ///        &lt;Fields&gt;
        ///     [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ILRTableMap_1819 {
            get {
                return ResourceManager.GetString("ILRTableMap_1819", resourceCulture);
            }
        }
    }
}
