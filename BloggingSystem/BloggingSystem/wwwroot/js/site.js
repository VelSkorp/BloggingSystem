$(document).ready(function () {
	$('#createPostForm').on('submit', function (event) {
		event.preventDefault();

		var formData = new FormData(this);
		var button = $('#submitButton');
		var spinner = $('#spinner');

		button.prop('disabled', true);
		spinner.removeClass('d-none');

		$.ajax({
			type: 'POST',
			url: '/Posts/Create',
			data: formData,
			contentType: false,
			processData: false,
			success: function (response) {
				window.location.href = '/';
			},
			error: function (error) {
				alert('An error occurred while submitting the form.');
				button.prop('disabled', false);
				spinner.addClass('d-none');
			}
		});
	});

	// Add click event to the camera icon to trigger file input
	document.querySelector('.camera-icon').addEventListener('click', function () {
		document.getElementById('photoInput').click();
	});

	$('#updateUserForm').on('submit', function (event) {
		event.preventDefault();

		var formData = new FormData(this);

		$.ajax({
			type: 'POST',
			url: '/Users/UpdateUser',
			data: formData,
			contentType: false,
			processData: false,
			success: function (response) {
				// Update the displayed fields
				document.getElementById('firstNameText').innerText = updatedData.firstName;
				document.getElementById('usernameText').innerText = `(${updatedData.username})`;
				document.getElementById('lastNameText').innerText = updatedData.lastName;
				// Hide the edit fields after saving
				document.getElementById('editFirstName').style.display = 'none';
				document.getElementById('editUsername').style.display = 'none';
				document.getElementById('editLastName').style.display = 'none';
			},
			error: function (error) {
				alert('An error occurred while updating user details.');
			}
		});
	});

	$('.commentForm').on('submit', function (event) {
		event.preventDefault();

		var postId = $(this).data('post-id');
		var commentContent = $('#commentContent-' + postId).val();

		$.ajax({
			type: 'POST',
			url: '/Posts/AddComment',
			data: { postId: postId, commentContent: commentContent },
			success: function (response) {
				$('#commentsContainer-' + postId).append(`
						<li>${response.comment.content} (${response.comment.author} on ${new Date(response.comment.createdAt).toLocaleDateString('en-US', { year: 'numeric', month: 'long', day: 'numeric' })})</li>
					`);
				$('#commentContent-' + postId).val('');
			},
			error: function () {
				alert('An error occurred while submitting the comment.');
			}
		});
	});

	$('.deletePost').on('click', function (event) {
		event.preventDefault();
	
		var postId = $(this).data('post-id');
	
		$.ajax({
			type: 'POST',
			url: '/Posts/Delete',
			data: { postId: postId },
			success: function (response) {
				$('#post-' + postId).remove();
			},
			error: function () {
				alert('An error occurred while deleting the post.');
			}
		});
	});
});

function uploadImage() {
	var fileInput = document.getElementById('fileInput');
	var file = fileInput.files[0];

	if (file) {
		var formData = new FormData();
		formData.append('photo', file);

		// Send an AJAX request to upload the photo
		$.ajax({
			type: 'POST',
			url: '/YourController/UploadPhoto', // Adjust to your endpoint
			data: formData,
			processData: false,
			contentType: false,
			success: function (response) {
				// Update the profile photo with the new image
				document.querySelector('.profile-photo').src = response.newImageUrl; // Ensure your response returns the new image URL
			},
			error: function () {
				alert('An error occurred while uploading the image.');
			}
		});
	}
}

function editField(field) {
	// Show the editing fields
	if (field === 'firstName') {
		document.getElementById('editFirstName').style.display = 'block';
		document.getElementById('firstNameText').style.display = 'none';
		document.getElementById('editFirstName').value = document.getElementById('firstNameText').innerText;
	} else if (field === 'username') {
		document.getElementById('editUsername').style.display = 'block';
		document.getElementById('usernameText').style.display = 'none';
		document.getElementById('editUsername').value = document.getElementById('usernameText').innerText.replace(/[()]/g, '').trim();
	} else if (field === 'lastName') {
		document.getElementById('editLastName').style.display = 'block';
		document.getElementById('lastNameText').style.display = 'none';
		document.getElementById('editLastName').value = document.getElementById('lastNameText').innerText;
	}
}