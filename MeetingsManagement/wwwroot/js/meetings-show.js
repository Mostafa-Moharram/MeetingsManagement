const meetingCards = document.querySelectorAll(".content .container .meetings .meeting");

meetingCards.forEach((meetingCard) => {
    meetingCard.querySelector(".delete-button").addEventListener("click", () => {
        const index = meetingCard.getElementsByTagName("input")[0].value;
        sendDeleteMeeting(index, () => meetingCard.remove());
    });
});

const sendDeleteMeeting = (index, onSuccess, onFailure) => {
    fetch("/Meetings/Delete", {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json"
        },
        body: index
    }).then(response => {
        if (response.ok) {
            if (onSuccess)
                onSuccess();
        } else {
            if (onFailure)
                onFailure();
        }
    });
};