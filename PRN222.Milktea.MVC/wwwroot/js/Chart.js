<script>
    var accountsCtx = document.getElementById('accountsChart').getContext('2d');
    var accountsChart = new Chart(accountsCtx, {
        type: 'pie',
    data: {
        labels: ['Total Registered', 'Total Banned'],
    datasets: [{
        label: 'Accounts Overview',
    data: [@Model.TotalRegisteredAccounts, @Model.TotalBannedAccounts],
    backgroundColor: ['#007bff', '#dc3545'],
    borderWidth: 1
            }]
        },
    options: {
        responsive: true, // Đảm bảo biểu đồ tự động thay đổi kích thước
    plugins: {
        legend: {
        position: 'top',
    labels: {
        font: {
        size: 14,
    family: 'Arial, sans-serif'
                        }
                    }
                },
    tooltip: {
        backgroundColor: '#fff',
    titleColor: '#333',
    bodyColor: '#007bff'
                }
            }
        }
    });

    var categoriesCtx = document.getElementById('categoriesChart').getContext('2d');
    var categoriesChart = new Chart(categoriesCtx, {
        type: 'bar',
    data: {
        labels: @Html.Raw(Json.Serialize(Model.Categories.Select(c => c.Name).ToArray())),
    datasets: [{
        label: 'Categories',
                data: @Html.Raw(Json.Serialize(Model.Categories.Select(c => c.IsActive.HasValue ? (c.IsActive.Value ? 1 : 0) : 0).ToArray())),
    backgroundColor: '#28a745',
    borderWidth: 1
            }]
        },
    options: {
        responsive: true,
    scales: {
        y: {
        beginAtZero: true
                }
            },
    plugins: {
        tooltip: {
        backgroundColor: '#fff',
    titleColor: '#333',
    bodyColor: '#007bff'
                }
            }
        }
    });

    var revenueCtx = document.getElementById('revenueChart').getContext('2d');
    var revenueChart = new Chart(revenueCtx, {
        type: 'doughnut',
    data: {
        labels: ['Revenue'],
    datasets: [{
        label: 'Total Revenue',
    data: [@Model.TotalRevenue],
    backgroundColor: '#007bff',
    borderWidth: 1
            }]
        },
    options: {
        responsive: true,
    plugins: {
        legend: {
        position: 'top',
    labels: {
        font: {
        size: 14,
    family: 'Arial, sans-serif'
                        }
                    }
                },
    tooltip: {
        backgroundColor: '#fff',
    titleColor: '#333',
    bodyColor: '#007bff'
                }
            }
        }
    });
</script>
