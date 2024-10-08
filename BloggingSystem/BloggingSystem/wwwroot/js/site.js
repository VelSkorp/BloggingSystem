﻿$(document).ready(function () {
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
				if (!response.success) {
					window.location.href = '/Posts/Error?message=' + response.message;
					return;
				}
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
	$('.camera-icon').on('click', function () {
		$('#photoInput').click();
	});

	$('#photoInput').on('change', function () {
		$('#updateUserForm').submit();
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
				if (!response.success) {
					window.location.href = '/Posts/Error?message=' + response.message;
					return;
				}
				// Update the displayed fields only if the response fields are not null
				if (response.photo !== null) {
					document.querySelector('.profile-photo').src = response.photo;
					document.querySelector('.profile-icon img').src = response.photo;
				}
				if (response.firstName !== null) {
					document.getElementById('firstNameText').innerText = response.firstName;
				}
				if (response.lastName !== null) {
					document.getElementById('lastNameText').innerText = response.lastName;
				}
				// Hide the edit fields after saving
				document.getElementById('editFirstName').style.display = 'none';
				document.getElementById('firstNameText').style.display = 'block';
				document.getElementById('editLastName').style.display = 'none';
				document.getElementById('lastNameText').style.display = 'block';
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
				if (!response.success) {
					window.location.href = '/Posts/Error?message=' + response.message;
					return;
				}
				$('#commentsContainer-' + postId)
					.append(`
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
				if (!response.success) {
					window.location.href = '/Posts/Error?message=' + response.message;
					return;
				}
				$('#post-' + postId).remove();
			},
			error: function () {
				alert('An error occurred while deleting the post.');
			}
		});
	});

	$('#subscription-button').on('click', function (event) {
		event.preventDefault();
		var button = document.getElementById('subscription-button');
		var author = document.getElementById('author').innerText.replace(/[()]/g, '');
		var isSubscribed = button.innerText === 'Unsubscribe';

		$.ajax({
			type: 'POST',
			url: '/Users/ToggleSubscription',
			data: { isSubscribed: isSubscribed, author: author },
			success: function (response) {
				if (!response.success) {
					window.location.href = '/Posts/Error?message=' + response.message;
					return;
				}
				button.innerText = isSubscribed ? 'Subscribe' : 'Unsubscribe';
				if (!isSubscribed) {
					$('#subscriptionContainer')
						.append(`
						<li id="subscription-${response.subscription.username}" class="d-flex align-items-center">
							<a asp-controller="Users" asp-action="AuthorDetails" asp-route-author="${response.subscription.username}">
								<img src="${response.subscription.photo}" class="subscription-icon" alt="subscription" />
								${response.subscription.username}
							</a>
						</li>
						`);
				}
				else {
					$('#subscription-' + author).remove();
				}
			},
			error: function () {
				alert('An error occurred while deleting the post.');
			}
		});
	});

	$('.remove-notification').on('click', function (event) {
		event.preventDefault();

		var subscriber = $(this).data('subscriber');
		var notification = $(this).data('notification');
		var $notificationItem = $(this);

		$.ajax({
			type: 'POST',
			url: '/Users/RemoveNotification',
			data: {
				subscriber: subscriber,
				notification: notification
			},
			success: function (response) {
				if (!response.success) {
					window.location.href = '/Posts/Error?message=' + response.message;
					return;
				}
				$notificationItem.remove();
			},
			error: function () {
				alert('An error occurred while trying to remove the notification.');
			}
		});
	});
});

function editField(field) {
	// Show the editing fields
	if (field === 'firstName') {
		document.getElementById('editFirstName').style.display = 'block';
		document.getElementById('firstNameText').style.display = 'none';
		document.getElementById('editFirstName').value = document.getElementById('firstNameText').innerText;
	} else if (field === 'lastName') {
		document.getElementById('editLastName').style.display = 'block';
		document.getElementById('lastNameText').style.display = 'none';
		document.getElementById('editLastName').value = document.getElementById('lastNameText').innerText;
	}
}

function toggleSidebar() {
	var sidebar = document.getElementById("sidebar");
	var mainContent = document.getElementById("main-content");
	var toggleBtn = document.querySelector(".toggle-btn");

	sidebar.classList.toggle("hidden");
	mainContent.classList.toggle("expanded");
	toggleBtn.classList.toggle("collapsed");

	if (sidebar.classList.contains("hidden")) {
		toggleBtn.innerHTML = "&#x22D9;"; // Right arrow
	}
	else {
		toggleBtn.innerHTML = "&#x22D8;"; // Left arrow
	}
}