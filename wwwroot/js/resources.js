function showContent(type) {
    let content = "";

    // Remove active class from all sidebar options
    const sidebarOptions = document.querySelectorAll("#sidebar ul li a");
    sidebarOptions.forEach(option => {
        option.classList.remove("active");
    });

    // Add active class to the clicked sidebar option
    event.target.classList.add("active");

    switch (type) {
        case 'insurance':
            content = `
            <div style="display: flex; align-items: center;">
                <div style="flex: 1;">
                    <h2>Insuring Your Car</h2>
                    <p>When insuring your first car after passing your driving test, it's crucial to consider several factors to ensure you get the right coverage at a reasonable price.</p>
                    <p>Firstly, it's essential to shop around and compare quotes from different insurance providers. Prices can vary significantly between insurers, so taking the time to research and obtain multiple quotes can help you find the best deal.</p>
                    <p>Additionally, consider the type of coverage you need. While it may be tempting to opt for the cheapest policy available, it's essential to ensure that you have adequate coverage for your needs.</p>
                    <p>Comparison sites are invaluable tools for consumers seeking the best deals on a wide range of products and services. These platforms allow users to compare prices, features, and reviews from multiple providers in one convenient location, saving both time and effort. Whether searching for insurance policies, electronics, travel accommodations, or financial products, comparison sites offer transparency and accessibility, empowering consumers to make informed decisions based on their specific needs and preferences.</p> 
                    <p>By presenting a comprehensive overview of available options, including price disparities and product specifications, these platforms facilitate smart shopping and help users secure the best value for their money. Additionally, comparison sites often feature user-generated reviews and ratings, providing valuable insights into the quality and reliability of products and services, further enhancing the decision-making process.</p> 
                    <p>Overall, comparison sites play a vital role in today's consumer landscape by promoting competition, driving down prices, and empowering individuals to make well-informed purchasing decisions.</p>
                    <h4>Links To Insurance Comparison Sites</h4>
                    <p><a href="https://www.comparethemarket.com" target="_blank">Compare the Market</a></p>
                    <p><a href="https://www.confused.com" target="_blank">Confused.com</a></p>
                    <p><a href="https://www.gocompare.com" target="_blank">GoCompare</a></p>
                    <p><a href="https://www.moneysupermarket.com" target="_blank">MoneySuperMarket</a></p>
                    <p><a href="https://www.uswitch.com" target="_blank">uSwitch</a></p>
                </div>
                <img src="/img/Insurance.png" style="width: 150px; margin-left: 20px;" alt="Insurance Image">
            </div>
        `;
            break;

        case 'theory':
            content = `
            <div style="display: flex; flex-direction: column;">
                <div>
                    <h2>UK Theory Test</h2>
                    <p>The UK car driving theory test is a mandatory examination for individuals looking to obtain a full driving licence. It consists of two parts: the multiple-choice section and the hazard perception test.</p>
                    <p>1. <strong>Multiple-choice section:</strong> This part assesses your knowledge of the Highway Code, road signs, and general driving principles. It includes questions covering various topics such as road safety, vehicle handling, and driving regulations. You'll need to select the correct answer from a set of options.</p>
                    <p>2. <strong>Hazard perception test:</strong> In this section, you'll watch a series of video clips that simulate real driving scenarios. Your task is to identify developing hazards, such as pedestrians crossing the road or vehicles changing lanes, by clicking the mouse when you spot them. The earlier you identify the hazard, the more points you score.</p>
                    <p>To pass the theory test, you must score at least 43 out of 50 in the multiple-choice section and at least 44 out of 75 in the hazard perception test. Once you pass the theory test, you can then book and take the practical driving test to obtain your full driving licence in the UK.</p>
                </div>
                <div class="question">
                    <div class="question-content">
                        <h2>Trial Questions</h2>
                        <p>What does this road sign mean?</p>
                        <ul>
                            <li><label><input type="radio" name="answer1" value="a"><span style="margin-right: 10px;"> A. Change lane</span></label></li>
                            <li><label><input type="radio" name="answer1" value="b"><span style="margin-right: 10px;"> B. Dual carriageway ends</span></label></li>
                            <li><label><input type="radio" name="answer1" value="c"><span style="margin-right: 10px;"> C. Roadworks ahead</span></label></li>
                        </ul>
                        <button onclick="checkAnswer('answer1', 'b')">Check Answer</button>
                        <p class="result"></p>
                    </div>
                    <div class="question-image">
                        <img src="/img/roadsign2.jpg" style="width: 150px;" alt="Roadsign2 Image">
                    </div>
                </div>

                <div class="question">
                    <div class="question-content">
                        <p>What is the national speed limit on UK country roads?</p>
                        <ul>
                            <li><label><input type="radio" name="answer2" value="a"><span style="margin-right: 10px;"> A. 30 mph</span></label></li>
                            <li><label><input type="radio" name="answer2" value="b"><span style="margin-right: 10px;"> B. 50 mph</span></label></li>
                            <li><label><input type="radio" name="answer2" value="c"><span style="margin-right: 10px;"> C. 60 mph</span></label></li>
                        </ul>
                        <button onclick="checkAnswer('answer2', 'c')">Check Answer</button>
                        <p class="result"></p>
                    </div>
                    <div class="question-image">
                        <img src="/img/roadsign3.png" style="width: 150px;" alt="Roadsign3 Image">
                    </div>
                </div>

                <div class="question">
                    <div class="question-content">
                        <p>What should you do if your car starts to skid?</p>
                        <ul>
                            <li><label><input type="radio" name="skidAnswer" value="a"><span style="margin-right: 10px;"> A. Brake firmly</span></label></li>
                            <li><label><input type="radio" name="skidAnswer" value="b"><span style="margin-right: 10px;"> B. Steer sharply</span></label></li>
                            <li><label><input type="radio" name="skidAnswer" value="c"><span style="margin-right: 10px;"> C. Steer into skid</span></label></li>
                        </ul>
                        <button onclick="checkAnswer('skidAnswer', 'c')">Check Answer</button>
                        <p class="result"></p>
                    </div>
                    <div class="question-image">
                        <img src="/img/roadsign.jpg" style="width: 150px;" alt="Skidding Car Image">
                    </div>
                </div>

                <h2>Additional Resources</h2>
                <p>For more information and to book your theory test for real, <a href="https://www.gov.uk/book-theory-test" target="_blank">click here</a>.</p>

                <p>For PC users, you can take a UK Government approved practice test here:</p>
                <ul class="download-links">
                    <li><a href="https://www.gov.uk/take-practice-theory-test" target="_blank">Take a practice test</a></li>
                </ul>

                <p>For mobile users, you can download the official DVSA practice test app:</p>
                <ul class="download-links">
                    <li><a href="https://apps.apple.com/gb/app/driving-theory-test-4-in-1-kit/id829581836" target="_blank">Download on the App Store</a></li>
                    <li><a href="https://play.google.com/store/apps/details?id=uk.co.focusmm.DTSCombo&referrer=utm_source%3Ddts%26utm_medium%3Dreferral%26utm_campaign%3DDTS_Store_PP%26anid%3Dadmob" target="_blank">Get it on Google Play</a></li>
                </ul>

            </div>
            `;
            break;

        case 'practical':
            content = `
                <div style="display: flex; flex-direction: column; align-items: justify;">
                <h2>Practical Driving Test</h2>
                    <p>The practical driving test is a significant milestone in the journey toward obtaining a full driving licence in the UK. This examination, overseen by a qualified driving examiner, serves as a comprehensive evaluation of a candidate's ability to operate a vehicle safely and confidently in real-world driving scenarios.</p>

                    <h4>Duration:</h4>
                    <p>The practical driving test typically lasts around 40 minutes. During this time, candidates are required to demonstrate their driving skills and knowledge across a variety of road and traffic conditions.</p>

                    <h4>Maneuvers:</h4>
                    <p>One key aspect of the practical driving test is the demonstration of various maneuvers. These maneuvers may include parallel parking, reversing around a corner, and potentially performing an emergency stop. Candidates must execute these maneuvers with precision and confidence, showcasing their ability to handle common driving challenges. Here are some examples of maneuvers you may be asked to perform:</p>
                    <table class="practical-videos">
                        <tr>
                            <td>Parallel Parking</td>
                            <td><iframe width="560" height="315" src="https://www.youtube.com/embed/JwmI6BfRl6M?si=HdayIISei1hmbiGp" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" allowfullscreen></iframe></td>
                        </tr>
                        <tr>
                            <td>Emergency Stop</td>
                            <td><iframe width="560" height="315" src="https://www.youtube.com/embed/O_JtZCrd9jI?si=6zAOM9-KMw-vAEmc" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" allowfullscreen></iframe></td>
                        </tr>
                        <tr>
                            <td>Forward Bay Parking</td>
                            <td><iframe width="560" height="315" src="https://www.youtube.com/embed/2cKy1MHMz60?si=AXcB2TWIiF-6vuQJ" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" allowfullscreen></iframe></td>
                        </tr>
                        <tr>
                            <td>Reverse Bay Parking</td>
                            <td><iframe width="560" height="315" src="https://www.youtube.com/embed/8-TvBcf8GWI?si=PoE7fG6cu2Cf5whO" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" allowfullscreen></iframe></td>
                        </tr>
                        <tr>
                            <td>Pull up on right and reverse</td>
                            <td><iframe width="560" height="315" src="https://www.youtube.com/embed/6G3a8F8wkQI?si=Q4xXSwO6cJ0EORTg" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" allowfullscreen></iframe></td>
                        </tr>
                    </table>

                    <h4 style="padding-top:20px">Assessment:</h4>
                    <p>Throughout the test, the examiner closely observes the candidate's performance, assessing their ability to carry out maneuvers safely and effectively. Additionally, the examiner evaluates the candidate's overall control of the vehicle, awareness of other road users, and adherence to traffic laws and road signs. This holistic assessment provides a comprehensive evaluation of the candidate's readiness to drive independently on public roads.</p>

                    <h4>Passing Criteria:</h4>
                    <p>To successfully pass the practical driving test, candidates must demonstrate a high standard of driving competence. This includes meeting specific criteria outlined by the Driver and Vehicle Standards Agency (DVSA), the regulatory body responsible for setting and maintaining driving standards in the UK. Candidates must exhibit safe and confident driving behaviors while demonstrating an understanding of road rules and regulations.</p>

                    <p>Overall, the practical driving test represents a crucial step toward achieving full driving independence. By successfully completing this examination, candidates demonstrate their ability to drive responsibly and safely, paving the way for a lifetime of safe and enjoyable motoring.</p>
                </div>


                    
                    
                </div>
                `;
            break;


        case 'buying':
            content = "<p>Content for First Time Buying option</p>";
            break;

        default:
            content = "<p>Select an option from the sidebar.</p>";
            break;
    }

    document.getElementById("content").innerHTML = content;   

}

function checkAnswer(answerGroupName, correctAnswer) {
    var selectedAnswer = document.querySelector('input[name="' + answerGroupName + '"]:checked');
    var resultElement = selectedAnswer.closest('.question').querySelector('.result');

    if (!selectedAnswer) {
        resultElement.textContent = 'Please select an answer.';
        resultElement.classList.remove('correct', 'incorrect'); // Remove any existing result styling
        return;
    }

    if (selectedAnswer.value === correctAnswer) {
        resultElement.textContent = 'Correct!';
        resultElement.classList.remove('incorrect');
        resultElement.classList.add('correct');
    } else {
        resultElement.textContent = 'Incorrect. The correct answer is ' + correctAnswer.toUpperCase() + '.';
        resultElement.classList.remove('correct');
        resultElement.classList.add('incorrect');
    }
}




