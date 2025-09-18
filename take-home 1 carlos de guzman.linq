<Query Kind="Statements">
  <Connection>
    <ID>ac21d523-b4c5-43aa-9aac-333c0f902dc7</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>EXO</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>StartTed-2025-Sept</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

//Q1

ClubActivities
	.Where(a =>
		a.StartDate >= new DateTime(2025, 1, 1) &&
		(a.CampusVenue == null || a.CampusVenue.Location !="Scheduled Room") &&
		a.Name != "BTech Club Meeting")

	.OrderBy(a => a.StartDate)
	.ThenBy(a => a.CampusVenue.Location)
	.ThenBy(a => a.Club.ClubName)
	.ThenBy(a => a.Name)

	.Select(a => new
		{
			StartDate = a.StartDate,
			Location = a.CampusVenue == null ? null : a.CampusVenue.Location,
			Club = a.Club == null ? null : a.Club.ClubName,
			Activity = a.Name
		})
	.Dump();

//Q2

Programs
	.Select(p => new
		{
			School = p.SchoolCode == "SAMIT" ? "School of Advance Media and IT"
				   : p.SchoolCode == "SEET" ? "School of Electrical Engineering Technology"
				   : "Unknown",

			Program = p.ProgramName,
			RequiredCourseCount = p.ProgramCourses.Count(c => c.Required),
			OptionalCourseCount = p.ProgramCourses.Count(c => !c.Required)
		})

	.Where(p => p.RequiredCourseCount >= 22)
	.OrderBy(p => p.Program)
	.Dump();

//Q3 

Students
	.Where(s =>
		s.StudentPayments.Count() == 0 &&
		s.Countries.CountryName != "Canada")

	.OrderBy(s => s.LastName)
	.Select(s => new
		{
			s.StudentNumber,
			s.Countries.CountryName,
			FullName = s.FirstName + " " + s.LastName,
			ClubMembershipCount = s.ClubMembers.Count() == 0 ? "None" : s.ClubMembers.Count().ToString()
		})
	.Dump();

//Q4

Employees
	.Where(e =>
		e.Position.Description == "Instructor" &&
		e.ReleaseDate == null &&
		e.ClassOfferings.Count() > 0)

	.OrderByDescending(e => e.ClassOfferings.Count())
	.ThenBy(e => e.LastName)
	.Select(e => new
		{
			e.Program.ProgramName,
			FullName = e.FirstName + " " + e.LastName,
			WorkLoad = e.ClassOfferings.Count() > 24 ? "High"
					 : e.ClassOfferings.Count() > 8 ? "Med"
					 : "Low"
		})
	.Dump();

//Q5

Clubs
	.OrderByDescending(c => c.ClubMembers.Count())
	.Select(c => new
		{
			Supervisor = c.Employee == null ? "Unknown" : c.Employee.FirstName + " " + c.Employee.LastName,
			c.ClubName,
			MemberCount = c.ClubMembers.Count(),
			Activities = c.ClubActivities.Count() == 0 ? "None Schedule" : c.ClubActivities.Count().ToString()
		})
	.Dump();