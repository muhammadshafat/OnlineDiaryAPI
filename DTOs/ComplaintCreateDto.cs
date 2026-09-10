public class ComplaintCreateDto
{
    public int Sid { get; set; }
    public int Pid { get; set; }
    public int Cid { get; set; }

    public string ComplaintTo { get; set; }

    public int Tid { get; set; }          // nullable because only for Teacher
    public int SubjectId { get; set; }    // nullable because only for Teacher

    public string Category { get; set; }
    public string Description { get; set; }
}