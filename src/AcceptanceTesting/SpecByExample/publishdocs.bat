mkdir pubdocs
cd pubdocs

git clone https://%1:%2@github.com/SkillsFundingAgency/das-paymentsacceptancetesting.git --branch gh-pages

xcopy ..\das-payments-AcceptanceTests\AcceptanceTests\docs das-paymentsacceptancetesting /E /R /Y

git config  user.email "kalim.akbar@fasst.org.uk" 
git config  user.name "build agent"

cd das-paymentsacceptancetesting
git add --all
git commit -m "Update docs for release %RELEASE_RELEASENAME%"
git push origin gh-pages
