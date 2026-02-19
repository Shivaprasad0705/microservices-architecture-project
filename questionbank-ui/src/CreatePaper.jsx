import { useState, useEffect } from "react";

function CreatePaper() {
  const [subjects, setSubjects] = useState([]);
  const [subject, setSubject] = useState("");
  const [easy, setEasy] = useState(5);
  const [medium, setMedium] = useState(5);
  const [hard, setHard] = useState(5);
  const [questions, setQuestions] = useState([]);

  // 🔹 Load subjects from backend
  useEffect(() => {
    fetch("http://localhost:5281/api/questions/subjects")
      .then((res) => {
        if (!res.ok) throw new Error("Failed to fetch subjects");
        return res.json();
      })
      .then((data) => {
        setSubjects(data);
        if (data.length > 0) {
          setSubject(data[0]); // auto select first subject
        }
      })
      .catch((err) => {
        console.error(err);
        alert("Failed to load subjects");
      });
  }, []);

  // 🔹 Generate paper
  const generatePaper = async () => {
    if (!subject) {
      alert("Please select a subject");
      return;
    }

    try {
      const res = await fetch(
        "http://localhost:5281/api/questions/generate-paper",
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            subject,
            easyCount: easy,
            mediumCount: medium,
            hardCount: hard,
          }),
        }
      );

      if (!res.ok) {
        const err = await res.text();
        alert(err);
        return;
      }

      const data = await res.json();
      setQuestions(data.questions);
    } catch (error) {
      console.error("Fetch error:", error);
      alert("Network / API error");
    }
  };

  return (
    <div>
      <h2>Create Question Paper</h2>

      {/* SUBJECT DROPDOWN */}
      <label>Subject: </label>
      {subjects.length === 0 ? (
        <span> Loading subjects...</span>
      ) : (
        <select value={subject} onChange={(e) => setSubject(e.target.value)}>
          {subjects.map((s) => (
            <option key={s} value={s}>
              {s}
            </option>
          ))}
        </select>
      )}

      <br /><br />

      Easy:
      <input
        type="number"
        value={easy}
        onChange={(e) => setEasy(+e.target.value)}
      />

      Medium:
      <input
        type="number"
        value={medium}
        onChange={(e) => setMedium(+e.target.value)}
      />

      Hard:
      <input
        type="number"
        value={hard}
        onChange={(e) => setHard(+e.target.value)}
      />

      <br /><br />

      <button onClick={generatePaper} disabled={!subject}>
        Generate Paper
      </button>

      <hr />

      <h3>Generated Question Paper</h3>

      {questions.length === 0 ? (
        <p>No questions generated</p>
      ) : (
        <ol>
          {questions.map((q) => (
            <li key={q.id}>
              <b>{q.questionText}</b>
              <br />
              A. {q.optionA}
              <br />
              B. {q.optionB}
              <br />
              C. {q.optionC}
              <br />
              D. {q.optionD}
            </li>
          ))}
        </ol>
      )}
    </div>
  );
}

export default CreatePaper;
