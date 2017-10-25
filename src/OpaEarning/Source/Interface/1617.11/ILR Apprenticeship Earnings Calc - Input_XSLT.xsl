<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<session-data>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation" namespace="http://www.w3.org/2001/XMLSchema-instance">C:/PROGRA~2/Oracle/POLICY~1/bin/sessiondata.xsd</xsl:attribute>
			<entity>
				<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
				<xsl:attribute name="id">global</xsl:attribute>
				<xsl:attribute name="complete"><xsl:value-of select="string(((normalize-space('true') = 'true') or (normalize-space('true') = '1')))"/></xsl:attribute>
				<xsl:for-each select="global">
					<xsl:variable name="var_CollectionPeriod" select="@CollectionPeriod"/>
					<xsl:variable name="var_LARSVersion" select="@LARSVersion"/>
					<xsl:variable name="var_UKPRN" select="@UKPRN"/>
					<xsl:variable name="var_Year" select="@Year"/>
					<instance>
						<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
						<xsl:if test="string(boolean($var_CollectionPeriod)) != 'false'">
							<attribute>
								<xsl:attribute name="id">CollectionPeriod</xsl:attribute>
								<text-val>
									<xsl:value-of select="string($var_CollectionPeriod)"/>
								</text-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_LARSVersion)) != 'false'">
							<attribute>
								<xsl:attribute name="id">LARSVersion</xsl:attribute>
								<text-val>
									<xsl:value-of select="string($var_LARSVersion)"/>
								</text-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_UKPRN)) != 'false'">
							<attribute>
								<xsl:attribute name="id">UKPRN</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_UKPRN)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_Year)) != 'false'">
							<attribute>
								<xsl:attribute name="id">Year</xsl:attribute>
								<text-val>
									<xsl:value-of select="string($var_Year)"/>
								</text-val>
							</attribute>
						</xsl:if>


			<entity>
				<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
				<xsl:attribute name="id">Learner</xsl:attribute>
				<xsl:attribute name="complete"><xsl:value-of select="string(((normalize-space('true') = 'true') or (normalize-space('true') = '1')))"/></xsl:attribute>
				<xsl:for-each select="Learner">
					<xsl:variable name="var_DateOfBirth" select="@DateOfBirth"/>
					<xsl:variable name="var_LearnRefNumber" select="@LearnRefNumber"/>
					<xsl:variable name="var_ULN" select="@ULN"/>
					<instance>
						<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
						<xsl:if test="string(boolean($var_DateOfBirth)) != 'false'">
							<attribute>
								<xsl:attribute name="id">DateOfBirth</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_DateOfBirth)"/>
								</date-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_LearnRefNumber)) != 'false'">
							<attribute>
								<xsl:attribute name="id">LearnRefNumber</xsl:attribute>
								<text-val>
									<xsl:value-of select="string($var_LearnRefNumber)"/>
								</text-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_ULN)) != 'false'">
							<attribute>
								<xsl:attribute name="id">ULN</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_ULN)"/>
								</number-val>
							</attribute>
						</xsl:if>


			<entity>
				<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
				<xsl:attribute name="id">LearningDelivery</xsl:attribute>
				<xsl:attribute name="complete"><xsl:value-of select="string(((normalize-space('true') = 'true') or (normalize-space('true') = '1')))"/></xsl:attribute>
				<xsl:for-each select="LearningDelivery">
					<xsl:variable name="var_AimSeqNumber" select="@AimSeqNumber"/>
					<xsl:variable name="var_AimType" select="@AimType"/>
					<xsl:variable name="var_CompStatus" select="@CompStatus"/>
					<xsl:variable name="var_FrameworkCommonComponent" select="@FrameworkCommonComponent"/>
					<xsl:variable name="var_FworkCode" select="@FworkCode"/>
					<xsl:variable name="var_LearnActEndDate" select="@LearnActEndDate"/>
					<xsl:variable name="var_LearnAimRef" select="@LearnAimRef"/>
					<xsl:variable name="var_LearnPlanEndDate" select="@LearnPlanEndDate"/>
					<xsl:variable name="var_LearnStartDate" select="@LearnStartDate"/>
					<xsl:variable name="var_LrnDelFAM_EEF" select="@LrnDelFAM_EEF"/>
					<xsl:variable name="var_OrigLearnStartDate" select="@OrigLearnStartDate"/>
					<xsl:variable name="var_OtherFundAdj" select="@OtherFundAdj"/>
					<xsl:variable name="var_PriorLearnFundAdj" select="@PriorLearnFundAdj"/>
					<xsl:variable name="var_ProgType" select="@ProgType"/>
					<xsl:variable name="var_PwayCode" select="@PwayCode"/>
					<xsl:variable name="var_STDCode" select="@STDCode"/>
					<instance>
						<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
						<xsl:if test="string(boolean($var_AimSeqNumber)) != 'false'">
							<attribute>
								<xsl:attribute name="id">AimSeqNumber</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_AimSeqNumber)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_AimType)) != 'false'">
							<attribute>
								<xsl:attribute name="id">AimType</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_AimType)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_CompStatus)) != 'false'">
							<attribute>
								<xsl:attribute name="id">CompStatus</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_CompStatus)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_FrameworkCommonComponent)) != 'false'">
							<attribute>
								<xsl:attribute name="id">FrameworkCommonComponent</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_FrameworkCommonComponent)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_FworkCode)) != 'false'">
							<attribute>
								<xsl:attribute name="id">FworkCode</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_FworkCode)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_LearnActEndDate)) != 'false'">
							<attribute>
								<xsl:attribute name="id">LearnActEndDate</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_LearnActEndDate)"/>
								</date-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_LearnAimRef)) != 'false'">
							<attribute>
								<xsl:attribute name="id">LearnAimRef</xsl:attribute>
								<text-val>
									<xsl:value-of select="string($var_LearnAimRef)"/>
								</text-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_LearnPlanEndDate)) != 'false'">
							<attribute>
								<xsl:attribute name="id">LearnPlanEndDate</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_LearnPlanEndDate)"/>
								</date-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_LearnStartDate)) != 'false'">
							<attribute>
								<xsl:attribute name="id">LearnStartDate</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_LearnStartDate)"/>
								</date-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_LrnDelFAM_EEF)) != 'false'">
							<attribute>
								<xsl:attribute name="id">LrnDelFAM_EEF</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_LrnDelFAM_EEF)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_OrigLearnStartDate)) != 'false'">
							<attribute>
								<xsl:attribute name="id">OrigLearnStartDate</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_OrigLearnStartDate)"/>
								</date-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_OtherFundAdj)) != 'false'">
							<attribute>
								<xsl:attribute name="id">OtherFundAdj</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_OtherFundAdj)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_PriorLearnFundAdj)) != 'false'">
							<attribute>
								<xsl:attribute name="id">PriorLearnFundAdj</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_PriorLearnFundAdj)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_ProgType)) != 'false'">
							<attribute>
								<xsl:attribute name="id">ProgType</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_ProgType)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_PwayCode)) != 'false'">
							<attribute>
								<xsl:attribute name="id">PwayCode</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_PwayCode)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_STDCode)) != 'false'">
							<attribute>
								<xsl:attribute name="id">STDCode</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_STDCode)"/>
								</number-val>
							</attribute>
						</xsl:if>


			<entity>
				<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
				<xsl:attribute name="id">TrailblazerApprenticeshipFinancialRecord</xsl:attribute>
				<xsl:attribute name="complete"><xsl:value-of select="string(((normalize-space('true') = 'true') or (normalize-space('true') = '1')))"/></xsl:attribute>
				<xsl:for-each select="TrailblazerApprenticeshipFinancialRecord">
					<xsl:variable name="var_TBFinAmount" select="@TBFinAmount"/>
					<xsl:variable name="var_TBFinCode" select="@TBFinCode"/>
					<xsl:variable name="var_TBFinDate" select="@TBFinDate"/>
					<xsl:variable name="var_TBFinType" select="@TBFinType"/>
					<instance>
						<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
						<xsl:if test="string(boolean($var_TBFinAmount)) != 'false'">
							<attribute>
								<xsl:attribute name="id">TBFinAmount</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_TBFinAmount)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_TBFinCode)) != 'false'">
							<attribute>
								<xsl:attribute name="id">TBFinCode</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_TBFinCode)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_TBFinDate)) != 'false'">
							<attribute>
								<xsl:attribute name="id">TBFinDate</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_TBFinDate)"/>
								</date-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_TBFinType)) != 'false'">
							<attribute>
								<xsl:attribute name="id">TBFinType</xsl:attribute>
								<text-val>
									<xsl:value-of select="string($var_TBFinType)"/>
								</text-val>
							</attribute>
						</xsl:if>


					</instance>
				</xsl:for-each>
			</entity>
			<entity>
				<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
				<xsl:attribute name="id">LearningDeliveryFAM</xsl:attribute>
				<xsl:attribute name="complete"><xsl:value-of select="string(((normalize-space('true') = 'true') or (normalize-space('true') = '1')))"/></xsl:attribute>
				<xsl:for-each select="LearningDeliveryFAM">
					<xsl:variable name="var_LearnDelFAMCode" select="@LearnDelFAMCode"/>
					<xsl:variable name="var_LearnDelFAMDateFrom" select="@LearnDelFAMDateFrom"/>
					<xsl:variable name="var_LearnDelFAMDateTo" select="@LearnDelFAMDateTo"/>
					<xsl:variable name="var_LearnDelFAMType" select="@LearnDelFAMType"/>
					<instance>
						<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
						<xsl:if test="string(boolean($var_LearnDelFAMCode)) != 'false'">
							<attribute>
								<xsl:attribute name="id">LearnDelFAMCode</xsl:attribute>
								<text-val>
									<xsl:value-of select="string($var_LearnDelFAMCode)"/>
								</text-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_LearnDelFAMDateFrom)) != 'false'">
							<attribute>
								<xsl:attribute name="id">LearnDelFAMDateFrom</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_LearnDelFAMDateFrom)"/>
								</date-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_LearnDelFAMDateTo)) != 'false'">
							<attribute>
								<xsl:attribute name="id">LearnDelFAMDateTo</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_LearnDelFAMDateTo)"/>
								</date-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_LearnDelFAMType)) != 'false'">
							<attribute>
								<xsl:attribute name="id">LearnDelFAMType</xsl:attribute>
								<text-val>
									<xsl:value-of select="string($var_LearnDelFAMType)"/>
								</text-val>
							</attribute>
						</xsl:if>


					</instance>
				</xsl:for-each>
			</entity>
			<entity>
				<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
				<xsl:attribute name="id">Standard_LARS_ApprenticshipFunding</xsl:attribute>
				<xsl:attribute name="complete"><xsl:value-of select="string(((normalize-space('true') = 'true') or (normalize-space('true') = '1')))"/></xsl:attribute>
				<xsl:for-each select="Standard_LARS_ApprenticshipFunding">
					<xsl:variable name="var_StandardAF1618Incentive" select="@StandardAF1618Incentive"/>
					<xsl:variable name="var_StandardAFEffectiveFrom" select="@StandardAFEffectiveFrom"/>
					<xsl:variable name="var_StandardAFEffectiveTo" select="@StandardAFEffectiveTo"/>
					<xsl:variable name="var_StandardAFFundingCategory" select="@StandardAFFundingCategory"/>
					<xsl:variable name="var_StandardAFMaxEmployerLevyCap" select="@StandardAFMaxEmployerLevyCap"/>
					<xsl:variable name="var_StandardAFReservedValue1" select="@StandardAFReservedValue1"/>
					<xsl:variable name="var_StandardAFReservedValue2" select="@StandardAFReservedValue2"/>
					<instance>
						<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
						<xsl:if test="string(boolean($var_StandardAF1618Incentive)) != 'false'">
							<attribute>
								<xsl:attribute name="id">StandardAF1618Incentive</xsl:attribute>
								<currency-val>
									<xsl:value-of select="string($var_StandardAF1618Incentive)"/>
								</currency-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_StandardAFEffectiveFrom)) != 'false'">
							<attribute>
								<xsl:attribute name="id">StandardAFEffectiveFrom</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_StandardAFEffectiveFrom)"/>
								</date-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_StandardAFEffectiveTo)) != 'false'">
							<attribute>
								<xsl:attribute name="id">StandardAFEffectiveTo</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_StandardAFEffectiveTo)"/>
								</date-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_StandardAFFundingCategory)) != 'false'">
							<attribute>
								<xsl:attribute name="id">StandardAFFundingCategory</xsl:attribute>
								<text-val>
									<xsl:value-of select="string($var_StandardAFFundingCategory)"/>
								</text-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_StandardAFMaxEmployerLevyCap)) != 'false'">
							<attribute>
								<xsl:attribute name="id">StandardAFMaxEmployerLevyCap</xsl:attribute>
								<currency-val>
									<xsl:value-of select="string($var_StandardAFMaxEmployerLevyCap)"/>
								</currency-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_StandardAFReservedValue1)) != 'false'">
							<attribute>
								<xsl:attribute name="id">StandardAFReservedValue1</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_StandardAFReservedValue1)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_StandardAFReservedValue2)) != 'false'">
							<attribute>
								<xsl:attribute name="id">StandardAFReservedValue2</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_StandardAFReservedValue2)"/>
								</number-val>
							</attribute>
						</xsl:if>


					</instance>
				</xsl:for-each>
			</entity>
			<entity>
				<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
				<xsl:attribute name="id">Framework_LARS_ApprenticshipFunding</xsl:attribute>
				<xsl:attribute name="complete"><xsl:value-of select="string(((normalize-space('true') = 'true') or (normalize-space('true') = '1')))"/></xsl:attribute>
				<xsl:for-each select="Framework_LARS_ApprenticshipFunding">
					<xsl:variable name="var_FrameworkAF1618Incentive" select="@FrameworkAF1618Incentive"/>
					<xsl:variable name="var_FrameworkAFEffectiveFrom" select="@FrameworkAFEffectiveFrom"/>
					<xsl:variable name="var_FrameworkAFEffectiveTo" select="@FrameworkAFEffectiveTo"/>
					<xsl:variable name="var_FrameworkAFFundingCategory" select="@FrameworkAFFundingCategory"/>
					<xsl:variable name="var_FrameworkAFMaxEmployerLevyCap" select="@FrameworkAFMaxEmployerLevyCap"/>
					<xsl:variable name="var_FrameworkAFReservedValue1" select="@FrameworkAFReservedValue1"/>
					<xsl:variable name="var_FrameworkAFReservedValue2" select="@FrameworkAFReservedValue2"/>
					<instance>
						<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
						<xsl:if test="string(boolean($var_FrameworkAF1618Incentive)) != 'false'">
							<attribute>
								<xsl:attribute name="id">FrameworkAF1618Incentive</xsl:attribute>
								<currency-val>
									<xsl:value-of select="string($var_FrameworkAF1618Incentive)"/>
								</currency-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_FrameworkAFEffectiveFrom)) != 'false'">
							<attribute>
								<xsl:attribute name="id">FrameworkAFEffectiveFrom</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_FrameworkAFEffectiveFrom)"/>
								</date-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_FrameworkAFEffectiveTo)) != 'false'">
							<attribute>
								<xsl:attribute name="id">FrameworkAFEffectiveTo</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_FrameworkAFEffectiveTo)"/>
								</date-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_FrameworkAFFundingCategory)) != 'false'">
							<attribute>
								<xsl:attribute name="id">FrameworkAFFundingCategory</xsl:attribute>
								<text-val>
									<xsl:value-of select="string($var_FrameworkAFFundingCategory)"/>
								</text-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_FrameworkAFMaxEmployerLevyCap)) != 'false'">
							<attribute>
								<xsl:attribute name="id">FrameworkAFMaxEmployerLevyCap</xsl:attribute>
								<currency-val>
									<xsl:value-of select="string($var_FrameworkAFMaxEmployerLevyCap)"/>
								</currency-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_FrameworkAFReservedValue1)) != 'false'">
							<attribute>
								<xsl:attribute name="id">FrameworkAFReservedValue1</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_FrameworkAFReservedValue1)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_FrameworkAFReservedValue2)) != 'false'">
							<attribute>
								<xsl:attribute name="id">FrameworkAFReservedValue2</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_FrameworkAFReservedValue2)"/>
								</number-val>
							</attribute>
						</xsl:if>


					</instance>
				</xsl:for-each>
			</entity>
			<entity>
				<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
				<xsl:attribute name="id">LARS_FrameworkCmnComp</xsl:attribute>
				<xsl:attribute name="complete"><xsl:value-of select="string(((normalize-space('true') = 'true') or (normalize-space('true') = '1')))"/></xsl:attribute>
				<xsl:for-each select="LARS_FrameworkCmnComp">
					<xsl:variable name="var_LARSFrameworkCommonComponentCode" select="@LARSFrameworkCommonComponentCode"/>
					<xsl:variable name="var_LARSFrameworkCommonComponentEffectiveFrom" select="@LARSFrameworkCommonComponentEffectiveFrom"/>
					<xsl:variable name="var_LARSFrameworkCommonComponentEffectiveTo" select="@LARSFrameworkCommonComponentEffectiveTo"/>
					<instance>
						<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
						<xsl:if test="string(boolean($var_LARSFrameworkCommonComponentCode)) != 'false'">
							<attribute>
								<xsl:attribute name="id">LARSFrameworkCommonComponentCode</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_LARSFrameworkCommonComponentCode)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_LARSFrameworkCommonComponentEffectiveFrom)) != 'false'">
							<attribute>
								<xsl:attribute name="id">LARSFrameworkCommonComponentEffectiveFrom</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_LARSFrameworkCommonComponentEffectiveFrom)"/>
								</date-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_LARSFrameworkCommonComponentEffectiveTo)) != 'false'">
							<attribute>
								<xsl:attribute name="id">LARSFrameworkCommonComponentEffectiveTo</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_LARSFrameworkCommonComponentEffectiveTo)"/>
								</date-val>
							</attribute>
						</xsl:if>


					</instance>
				</xsl:for-each>
			</entity>
			<entity>
				<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
				<xsl:attribute name="id">LARS_StandardCommonComponent</xsl:attribute>
				<xsl:attribute name="complete"><xsl:value-of select="string(((normalize-space('true') = 'true') or (normalize-space('true') = '1')))"/></xsl:attribute>
				<xsl:for-each select="LARS_StandardCommonComponent">
					<xsl:variable name="var_LARSStandardCommonComponentCode" select="@LARSStandardCommonComponentCode"/>
					<xsl:variable name="var_LARSStandardCommonComponentEffectiveFrom" select="@LARSStandardCommonComponentEffectiveFrom"/>
					<xsl:variable name="var_LARSStandardCommonComponentEffectiveTo" select="@LARSStandardCommonComponentEffectiveTo"/>
					<instance>
						<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
						<xsl:if test="string(boolean($var_LARSStandardCommonComponentCode)) != 'false'">
							<attribute>
								<xsl:attribute name="id">LARSStandardCommonComponentCode</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_LARSStandardCommonComponentCode)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_LARSStandardCommonComponentEffectiveFrom)) != 'false'">
							<attribute>
								<xsl:attribute name="id">LARSStandardCommonComponentEffectiveFrom</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_LARSStandardCommonComponentEffectiveFrom)"/>
								</date-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_LARSStandardCommonComponentEffectiveTo)) != 'false'">
							<attribute>
								<xsl:attribute name="id">LARSStandardCommonComponentEffectiveTo</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_LARSStandardCommonComponentEffectiveTo)"/>
								</date-val>
							</attribute>
						</xsl:if>


					</instance>
				</xsl:for-each>
			</entity>
					</instance>
				</xsl:for-each>
			</entity>
			<entity>
				<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
				<xsl:attribute name="id">LearnerEmploymentStatus</xsl:attribute>
				<xsl:attribute name="complete"><xsl:value-of select="string(((normalize-space('true') = 'true') or (normalize-space('true') = '1')))"/></xsl:attribute>
				<xsl:for-each select="LearnerEmploymentStatus">
					<xsl:variable name="var_DateEmpStatApp" select="@DateEmpStatApp"/>
					<xsl:variable name="var_EmpId" select="@EmpId"/>
					<xsl:variable name="var_EMPStat" select="@EMPStat"/>
					<xsl:variable name="var_EmpStatMon_EII" select="@EmpStatMon_EII"/>
					<xsl:variable name="var_EmpStatMon_SEM" select="@EmpStatMon_SEM"/>
					<instance>
						<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
						<xsl:if test="string(boolean($var_DateEmpStatApp)) != 'false'">
							<attribute>
								<xsl:attribute name="id">DateEmpStatApp</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_DateEmpStatApp)"/>
								</date-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_EmpId)) != 'false'">
							<attribute>
								<xsl:attribute name="id">EmpId</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_EmpId)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_EMPStat)) != 'false'">
							<attribute>
								<xsl:attribute name="id">EMPStat</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_EMPStat)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_EmpStatMon_EII)) != 'false'">
							<attribute>
								<xsl:attribute name="id">EmpStatMon_EII</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_EmpStatMon_EII)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_EmpStatMon_SEM)) != 'false'">
							<attribute>
								<xsl:attribute name="id">EmpStatMon_SEM</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_EmpStatMon_SEM)"/>
								</number-val>
							</attribute>
						</xsl:if>


					</instance>
				</xsl:for-each>
			</entity>
			<entity>
				<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
				<xsl:attribute name="id">SFA_PostcodeDisadvantage</xsl:attribute>
				<xsl:attribute name="complete"><xsl:value-of select="string(((normalize-space('true') = 'true') or (normalize-space('true') = '1')))"/></xsl:attribute>
				<xsl:for-each select="SFA_PostcodeDisadvantage">
					<xsl:variable name="var_DisUpEffectiveFrom" select="@DisUpEffectiveFrom"/>
					<xsl:variable name="var_DisUpEffectiveTo" select="@DisUpEffectiveTo"/>
					<xsl:variable name="var_DisUplift" select="@DisUplift"/>
					<instance>
						<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
						<xsl:if test="string(boolean($var_DisUpEffectiveFrom)) != 'false'">
							<attribute>
								<xsl:attribute name="id">DisUpEffectiveFrom</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_DisUpEffectiveFrom)"/>
								</date-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_DisUpEffectiveTo)) != 'false'">
							<attribute>
								<xsl:attribute name="id">DisUpEffectiveTo</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_DisUpEffectiveTo)"/>
								</date-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_DisUplift)) != 'false'">
							<attribute>
								<xsl:attribute name="id">DisUplift</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_DisUplift)"/>
								</number-val>
							</attribute>
						</xsl:if>


					</instance>
				</xsl:for-each>
			</entity>
			<entity>
				<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
				<xsl:attribute name="id">HistoricEarningInput</xsl:attribute>
				<xsl:attribute name="complete"><xsl:value-of select="string(((normalize-space('true') = 'true') or (normalize-space('true') = '1')))"/></xsl:attribute>
				<xsl:for-each select="HistoricEarningInput">
					<xsl:variable name="var_AppIdentifierInput" select="@AppIdentifierInput"/>
					<xsl:variable name="var_HistoricCollectionReturnInput" select="@HistoricCollectionReturnInput"/>
					<xsl:variable name="var_HistoricCollectionYearInput" select="@HistoricCollectionYearInput"/>
					<xsl:variable name="var_HistoricDaysInYearInput" select="@HistoricDaysInYearInput"/>
					<xsl:variable name="var_HistoricFworkCodeInput" select="@HistoricFworkCodeInput"/>
					<xsl:variable name="var_HistoricLearnRefNumberInput" select="@HistoricLearnRefNumberInput"/>
					<xsl:variable name="var_HistoricProgTypeInput" select="@HistoricProgTypeInput"/>
					<xsl:variable name="var_HistoricPwayCodeInput" select="@HistoricPwayCodeInput"/>
					<xsl:variable name="var_HistoricSTDCodeInput" select="@HistoricSTDCodeInput"/>
					<xsl:variable name="var_HistoricUKPRNInput" select="@HistoricUKPRNInput"/>
					<xsl:variable name="var_HistoricULNInput" select="@HistoricULNInput"/>
					<xsl:variable name="var_HistoricUptoEndDateInput" select="@HistoricUptoEndDateInput"/>
					<instance>
						<xsl:attribute name="id"><xsl:value-of select="generate-id(.)"/></xsl:attribute>
						<xsl:if test="string(boolean($var_AppIdentifierInput)) != 'false'">
							<attribute>
								<xsl:attribute name="id">AppIdentifierInput</xsl:attribute>
								<text-val>
									<xsl:value-of select="string($var_AppIdentifierInput)"/>
								</text-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_HistoricCollectionReturnInput)) != 'false'">
							<attribute>
								<xsl:attribute name="id">HistoricCollectionReturnInput</xsl:attribute>
								<text-val>
									<xsl:value-of select="string($var_HistoricCollectionReturnInput)"/>
								</text-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_HistoricCollectionYearInput)) != 'false'">
							<attribute>
								<xsl:attribute name="id">HistoricCollectionYearInput</xsl:attribute>
								<text-val>
									<xsl:value-of select="string($var_HistoricCollectionYearInput)"/>
								</text-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_HistoricDaysInYearInput)) != 'false'">
							<attribute>
								<xsl:attribute name="id">HistoricDaysInYearInput</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_HistoricDaysInYearInput)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_HistoricFworkCodeInput)) != 'false'">
							<attribute>
								<xsl:attribute name="id">HistoricFworkCodeInput</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_HistoricFworkCodeInput)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_HistoricLearnRefNumberInput)) != 'false'">
							<attribute>
								<xsl:attribute name="id">HistoricLearnRefNumberInput</xsl:attribute>
								<text-val>
									<xsl:value-of select="string($var_HistoricLearnRefNumberInput)"/>
								</text-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_HistoricProgTypeInput)) != 'false'">
							<attribute>
								<xsl:attribute name="id">HistoricProgTypeInput</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_HistoricProgTypeInput)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_HistoricPwayCodeInput)) != 'false'">
							<attribute>
								<xsl:attribute name="id">HistoricPwayCodeInput</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_HistoricPwayCodeInput)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_HistoricSTDCodeInput)) != 'false'">
							<attribute>
								<xsl:attribute name="id">HistoricSTDCodeInput</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_HistoricSTDCodeInput)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_HistoricUKPRNInput)) != 'false'">
							<attribute>
								<xsl:attribute name="id">HistoricUKPRNInput</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_HistoricUKPRNInput)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_HistoricULNInput)) != 'false'">
							<attribute>
								<xsl:attribute name="id">HistoricULNInput</xsl:attribute>
								<number-val>
									<xsl:value-of select="string($var_HistoricULNInput)"/>
								</number-val>
							</attribute>
						</xsl:if>
						<xsl:if test="string(boolean($var_HistoricUptoEndDateInput)) != 'false'">
							<attribute>
								<xsl:attribute name="id">HistoricUptoEndDateInput</xsl:attribute>
								<date-val>
									<xsl:value-of select="string($var_HistoricUptoEndDateInput)"/>
								</date-val>
							</attribute>
						</xsl:if>


					</instance>
				</xsl:for-each>
			</entity>
					</instance>
				</xsl:for-each>
			</entity>
					</instance>
				</xsl:for-each>
			</entity>

		</session-data>
	</xsl:template>
</xsl:stylesheet>
