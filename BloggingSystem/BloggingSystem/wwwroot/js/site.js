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
			url: 'Create',
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

	$('#commentForm').on('submit', function (event) {
		event.preventDefault();
	
		var postId = $(this).data('post-id');
		var commentContent = $('#commentContent').val();
	
		$.ajax({
			type: 'POST',
			url: '@Url.Action("AddComment", "Posts")',
			data: { postId: postId, commentContent: commentContent },
			success: function (response) {
				$('#commentsContainer').append(`
						<li>${response.comment.content} (${response.comment.author} on ${new Date(response.comment.createdAt).toLocaleDateString('en-US', { year: 'numeric', month: 'long', day: 'numeric' })})</li>
					`);
				$('#commentContent').val('');
			},
			error: function () {
				alert('An error occurred while submitting the comment.');
			}
		});
	});
});